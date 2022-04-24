using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using LiteDB;
using Microsoft.AspNetCore.Authorization;
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
	[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
	[Authorize("ApiScope")]
	public class MusicController : ControllerBase
	{
		private readonly IConfiguration Configuration;
		public const string RegexVideoId = ".*[\\?|&]v=([^&]*).*";

		public MusicController(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		[HttpGet("music-list")]
		public async Task<ActionResult<List<MusicDto>>> GetMusicList()
		{
			var musicList = new List<MusicDto>();

			var musicsFolderPath = Configuration["MusicsFolder"];

			var musics = Directory.GetFiles(musicsFolderPath, "*.mp3");

			foreach(var music in musics)
			{
				var file = TagLib.File.Create(music);

				musicList.Add(new MusicDto()
				{
					Artist = file.Tag.FirstPerformer,
					Title = file.Tag.Title
				});
			}

			return Ok(musicList);
		}

		[HttpPost("download")]
		public async Task<ActionResult<MusicDto>> Download(DownloadDto dto)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WorkingDirectory = Path.GetTempPath(),
					FileName = Configuration["YoutubeDLPath"],
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

			var finalFilename = $"{dto.Artist} - {dto.Title}.mp3";

			process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WorkingDirectory = musicsFolderPath,
					FileName = Configuration["ffmpegPath"],
					Arguments = $"-i \"{Path.Combine(Path.GetTempPath(), tempFilename)}\" -vn -ab 192k -ar 44100 -y \"{finalFilename}\"",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};
			process.Start();

			await process.WaitForExitAsync();

			System.IO.File.Delete(Path.Combine(Path.GetTempPath(), tempFilename));

			var file = TagLib.File.Create(Path.Combine(musicsFolderPath, finalFilename));
			file.Tag.Title = dto.Title;
			file.Tag.Performers = new string[] { dto.Artist };
			file.Save();

			var youtubeVideoId = Regex.Match(dto.Url, RegexVideoId).Groups[1].Value;

			using (var db = new LiteDatabase(Configuration["LiteDB"]))
			{
				var col = db.GetCollection<MusicDatabase>("musics");

				var music = new MusicDatabase
				{
					YoutubeId = youtubeVideoId
				};

				col.Insert(music);
			}

			return Ok(new MusicDto()
			{
				Artist = dto.Artist,
				Title = dto.Title,
				Id = youtubeVideoId
			});
		}

		[HttpGet("playlist")]
		public async Task<ActionResult<PlaylistResponseDto>> GetPlaylist(string pageToken = null)
		{
			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				ApiKey = Configuration["youtube:apiKey"],
				ApplicationName = "YoutubePlaylistToMp3"
			});

			var playlistRequest = youtubeService.PlaylistItems.List("snippet");
			playlistRequest.PlaylistId = Configuration["youtube:playlistId"];
			playlistRequest.MaxResults = 50;
			playlistRequest.PageToken = pageToken;
			playlistRequest.PrettyPrint = false;

			var response = await playlistRequest.ExecuteAsync();

			var dto = new PlaylistResponseDto()
			{
				Items = new List<PlaylistItemDto>(),
				NextPageToken = response.NextPageToken
			};

			List<MusicDatabase> musics;

			using (var db = new LiteDatabase(Configuration["LiteDB"]))
			{
				var col = db.GetCollection<MusicDatabase>("musics");

				musics = col.FindAll().ToList();
			}

			foreach (var element in response.Items)
			{
				dto.Items.Add(new PlaylistItemDto()
				{
					Id = element.Snippet.ResourceId.VideoId,
					Channel = element.Snippet.VideoOwnerChannelTitle,
					Downloaded = musics.Any(x => x.YoutubeId == element.Snippet.ResourceId.VideoId),
					Title = element.Snippet.Title,
					ThumbnailUrl = element.Snippet.Thumbnails.Default__?.Url,
					Url = "https://www.youtube.com/watch?v=" + element.Snippet.ResourceId.VideoId,
				});
			}

			youtubeService.Dispose();

			return Ok(dto);
		}

		public class PlaylistItemDto
		{
			public string Id { get; set; }
			public bool Downloaded { get; set; }
			public string Title { get; set; }
			public string Channel { get; set; }
			public string Url { get; set; }
			public string ThumbnailUrl { get; set; }
		}

		public class PlaylistResponseDto
		{
			public IList<PlaylistItemDto> Items { get; set; }
			public string NextPageToken { get; set; }
		}

		public class MusicDatabase
		{
			public int Id { get; set; }
			public string YoutubeId { get; set; }
		}

		public class MusicDto
		{
			public string Artist { get; set; }
			public string Title { get; set; }
			public string Id { get; set; }
		}

		public class DownloadDto
		{
			[Required(AllowEmptyStrings = false)]
			public string Title { get; set; }
			[Required(AllowEmptyStrings = false)]
			public string Artist { get; set; }
			[Required(AllowEmptyStrings = false)]
			[RegularExpression(RegexVideoId)]
			public string Url { get; set; }
		}
	}
}
