<template>
	<div id="playlist-item-container">
		<div id="playlist-item-content">
			<div id="playlist-item-image-container">
				<img  src="https://s.yimg.com/os/creatr-uploaded-images/2021-02/572c4830-721d-11eb-bb63-96959c3b62f2"/>
			</div>

			<div id="playlist-item-description">
				<h5>Rick Astley - Never Gonna Give You Up (Official Music Video)</h5>
				<p>Rick Astley</p>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component
export default class PlaylistItem extends Vue {
	@Prop() private isLoading! : boolean;
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
	@import 'bootstrap/scss/_functions.scss';
	@import 'bootstrap/scss/_variables.scss';
	@import 'bootstrap/scss/_mixins.scss';

	#playlist-item-container
	{
		height: 100px;
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
		width: 100%;
		height: 100%;
	}

	@include media-breakpoint-down(xs) {
		#playlist-item-image-container img
		{
			max-height: 60%;
		}
	}

	#playlist-item-description
	{
		margin-left: 10px;
	}

	#playlist-item-description h5
	{
		overflow: hidden;
		text-overflow: ellipsis;
		display: -webkit-box;
		-webkit-line-clamp: 2; /* number of lines to show */
		-webkit-box-orient: vertical;
	}

	#playlist-item-description p
	{
		color: rgb(170, 170, 170);
	}
</style>
