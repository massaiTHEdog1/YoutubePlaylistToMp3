<template>
	<b-container fluid>
		<b-row style="background-color: #212121;">
			<router-link to="/" exact class="nav-item">Playlist</router-link>
			
			<router-link to="/download" class="nav-item">Download</router-link>
			
			<router-link to="/music-list" class="nav-item">Music list</router-link>
		</b-row>
		<b-row>
			<b-col>
				<!-- <keep-alive> -->
					<router-view />
				<!-- </keep-alive> -->
			</b-col>
		</b-row>
	</b-container>
</template>

<style lang="scss" scoped>
	@import 'bootstrap/scss/_functions.scss';
	@import 'bootstrap/scss/_variables.scss';
	@import 'bootstrap/scss/_mixins.scss';

	@include media-breakpoint-up(xs) {
		.nav-item
		{
			width: 180px;
		}
	}
	@include media-breakpoint-down(xs) {
		.nav-item
		{
			width: 100%;
		}
	}

	.nav-item
	{
		height: 40px;
		line-height: 40px;
		text-align: center;
		color: white;
		text-decoration: none;
	}

	.nav-item:hover
	{
		background-color: #383838;
	}

	.router-link-active
	{
		font-weight: bold;
		background-color: #383838;
	}

	.router-link-active:hover
	{
		background-color: #4c4c4c;
	}
</style>
