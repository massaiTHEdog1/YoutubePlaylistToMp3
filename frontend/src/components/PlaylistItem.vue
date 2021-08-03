<template>
	<div id="playlist-item-container">
		<div id="playlist-item-content">
			<div id="playlist-item-image-container">
				<img v-bind:src="playlistItem.thumbnailUrl"/>
			</div>

			<div id="playlist-item-description">
				<h5>{{playlistItem.title}}</h5>
				<p>{{playlistItem.channel}}</p>
				<font-awesome-icon v-if="playlistItem.downloaded" icon="check-circle" style="color: green;" title="Downloaded" />
				<font-awesome-icon v-else-if="playlistItem.downloading" icon="spinner" spin size="lg" title="Downloading..." />
				<b-button v-else-if="playlistItem.title !== 'Deleted video' && playlistItem.title !== 'Private video'" variant="primary" size="sm" @click="$emit('downloadClick', playlistItem)">Download</b-button>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { PlaylistItemDto } from "@/classes/PlaylistItemDto";
import { Component, Prop, Vue } from "vue-property-decorator";

@Component
export default class PlaylistItem extends Vue {
	@Prop() playlistItem!: PlaylistItemDto;
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
	@import 'bootstrap/scss/_functions.scss';
	@import 'bootstrap/scss/_variables.scss';
	@import 'bootstrap/scss/_mixins.scss';

	#playlist-item-container
	{
		height: 120px;
		width: 100%;
		max-width: 900px;
		padding: 10px 20px 0px 0px;
	}

	#playlist-item-container:hover
	{
		background-color: #272727;
	}

	#playlist-item-content
	{
		height: 100%;
		width: 100%;
		border-bottom: 1px solid #272727;
		padding-bottom: 10px;
		display: flex;
	}

	#playlist-item-image-container
	{
		display:flex;
		justify-content:center;
		align-items:center;
		max-width: 100%;
		height: 100%;
	}

	#playlist-item-image-container > img
	{
		width: 105px;
		height: 79px;
	}

	@include media-breakpoint-down(xs) {
		#playlist-item-image-container img
		{
			width: 80px;
			height: 60px;
		}
	}

	#playlist-item-description
	{
		margin-left: 10px;
		overflow: hidden;
	}

	#playlist-item-description h5
	{
		line-height: 1.4;
		margin-bottom: 0px;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	#playlist-item-description p
	{
		color: rgb(170, 170, 170);
		margin-bottom: 10px;
	}
</style>
