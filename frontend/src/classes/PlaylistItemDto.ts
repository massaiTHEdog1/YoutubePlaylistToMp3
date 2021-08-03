export class PlaylistItemDto
{
	id! : string;
	downloaded! : boolean;
	downloading : boolean = false;
	title! : string;
	channel! : string;
	url! : string;
	thumbnailUrl! : string;

	public constructor(init?: Partial<PlaylistItemDto>) {
		Object.assign(this, init);
	 }
}