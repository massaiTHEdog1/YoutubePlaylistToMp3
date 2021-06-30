<template>
 	<b-container fluid>
		<h1>Playlist</h1>
		 
		<playlist-item/>
		
	</b-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import LoadingPane from "@/components/LoadingPane.vue"; // @ is an alias to /src
import PlaylistItem from "@/components/PlaylistItem.vue";

@Component({
  components: {
	LoadingPane,
	PlaylistItem
  },
})
export default class Playlist extends Vue {

	isLoadingPlaylist = false;

	async mounted() {
		await this.loadPlaylist();
	}

	async loadPlaylist() {
		this.isLoadingPlaylist = true;
		await this.timeout(3000);
		this.isLoadingPlaylist = false;
	}

	timeout(ms : number) {
		return new Promise(resolve => setTimeout(resolve, ms));
	}
}
</script>

<style lang="scss">
	
</style>