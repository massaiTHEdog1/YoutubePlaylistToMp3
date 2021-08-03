import { PlaylistItemDto } from "./PlaylistItemDto";

export class PlaylistResponseDto
{
	items! : PlaylistItemDto[];
	nextPageToken! : string;
}