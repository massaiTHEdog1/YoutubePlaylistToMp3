using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MusicController : ControllerBase
	{
		private readonly IConfiguration Configuration;

		public MusicController(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		[HttpGet("music-list")]
		public async Task<ActionResult<List<Music>>> GetMusicList()
		{
			var musicList = new List<Music>();

			var musicsFolderPath = Configuration["MusicsFolder"];

			var musics = Directory.GetFiles(musicsFolderPath, "*.mp3");

			foreach(var music in musics)
			{
				var file = TagLib.File.Create(music);

				musicList.Add(new Music()
				{
					Artist = file.Tag.FirstPerformer,
					Title = file.Tag.Title
				});
			}

			return Ok(musicList);
		}

		[HttpPost("download")]
		public async Task<ActionResult<Music>> Download(DownloadDto dto)
		{
			var context = new ValidationContext(dto);

			var validationResults = new List<ValidationResult>();

			bool isValid = Validator.TryValidateObject(dto, context, validationResults, true);

			if (!isValid)
				return BadRequest();

			var youtubeDLPath = Configuration["YoutubeDLPath"];

			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WorkingDirectory = Path.GetTempPath(),
					FileName = youtubeDLPath,
					Arguments = $"--extract-audio --audio-quality 0 --output \"{Guid.NewGuid()}.%(ext)s\" {dto.Url}",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			process.Start();

			StreamReader reader = process.StandardOutput;
			string output = await reader.ReadToEndAsync();

			await process.WaitForExitAsync();

			var tempFilename = Regex.Match(output, "\\[download\\] Destination: (.*)\n").Groups[1].Value;

			var musicsFolderPath = Configuration["MusicsFolder"];
			var ffmpegPath = Configuration["ffmpegPath"];

			var finalFilename = $"{dto.Artist} - {dto.Title}.mp3";

			process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WorkingDirectory = musicsFolderPath,
					FileName = ffmpegPath,
					Arguments = $"-i \"{Path.Combine(Path.GetTempPath(), tempFilename)}\" -vn -ab 192k -ar 44100 -y \"{finalFilename}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			process.Start();

			reader = process.StandardOutput;
			output = await reader.ReadToEndAsync();

			await process.WaitForExitAsync();

			System.IO.File.Delete(Path.Combine(Path.GetTempPath(), tempFilename));

			var file = TagLib.File.Create(Path.Combine(musicsFolderPath, finalFilename));
			file.Tag.Title = dto.Title;
			file.Tag.Performers = new string[] { dto.Artist };
			file.Save();

			return Ok(new Music()
			{
				Artist = dto.Artist,
				Title = dto.Title
			});
		}

		public class Music
		{
			public string Title { get; set; }
			public string Artist { get; set; }
		}

		public class DownloadDto
		{
			[Required(AllowEmptyStrings = false)]
			public string Title { get; set; }
			[Required(AllowEmptyStrings = false)]
			public string Artist { get; set; }
			[Required(AllowEmptyStrings = false)]
			public string Url { get; set; }
		}
	}
}
