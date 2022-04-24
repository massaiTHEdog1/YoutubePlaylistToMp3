<template>
  <b-container>
    <h1>Playlist</h1>

    <playlist-item
      v-for="item in playlistItems"
      v-bind:key="item.Id"
      v-bind:playlist-item="item"
      v-on:downloadClick="OnDownloadClick"
    />

    <b-button
      v-if="nextPageToken && !isLoading"
      style="margin: 10px auto; display: block"
      @click="loadPlaylist(nextPageToken)"
      >Load more</b-button
    >
    <font-awesome-icon
      v-if="isLoading"
      icon="spinner"
      spin
      size="lg"
      style="margin: 10px auto; display: block"
    />

    <b-modal
      id="modal-download"
      title="Download"
      header-bg-variant="dark"
      header-text-variant="light"
      body-bg-variant="dark"
      body-text-variant="light"
      footer-bg-variant="dark"
      footer-text-variant="light"
      ok-title="Download"
      v-on:ok="(e) => startDownload(e, currentItem, title, artist, url)"
    >
      <img
        :src="thumbnailUrl"
        style="width: 140px; margin: auto; display: block"
      />
      <b-form-group label="Youtube url:" label-for="input-1">
        <b-form-input
          readonly
          id="input-1"
          v-model="url"
          type="url"
          placeholder="Enter video url"
          required
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Artist:" label-for="input-2">
        <b-form-input
          id="input-2"
          v-model="artist"
          type="text"
          placeholder="Enter artist"
          required
        ></b-form-input>
      </b-form-group>

      <b-form-group label="Title:" label-for="input-3">
        <b-form-input
          id="input-3"
          v-model="title"
          type="text"
          placeholder="Enter title"
          required
        ></b-form-input>
      </b-form-group>
    </b-modal>
  </b-container>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import LoadingPane from "@/components/LoadingPane.vue"; // @ is an alias to /src
import PlaylistItem from "@/components/PlaylistItem.vue";
import axios from "axios";
import { EventBus } from "@/main";
import { PlaylistItemDto } from "@/classes/PlaylistItemDto";
import { PlaylistResponseDto } from "@/classes/PlaylistResponseDto";

@Component({
  components: {
    LoadingPane,
    PlaylistItem,
  },
})
export default class Playlist extends Vue {
  isLoading = false;

  playlistItems: PlaylistItemDto[] = [];
  nextPageToken = "";

  thumbnailUrl = "";
  url = "";
  artist = "";
  title = "";
  currentItem!: PlaylistItemDto;

  async mounted() {
    await this.loadPlaylist();
  }

  async loadPlaylist(pageToken?: string) {
    this.isLoading = true;

    try {
      const user = await this.$authService.mgr.getUser();
      axios.defaults.headers.common["Authorization"] =
        "Bearer " + user!.access_token;

      var result = await axios.get<PlaylistResponseDto>(
        process.env.VUE_APP_BASE_URL +
          "/api/music/playlist?pageToken=" +
          (pageToken ?? "")
      );

      result.data.items.forEach((element) => (element.downloading = false));
      this.playlistItems = this.playlistItems.concat(result.data.items);
      this.nextPageToken = result.data.nextPageToken;
    } catch (e) {
      EventBus.$emit(
        "addAlertToast",
        "Loading failed. Please refresh the page."
      );
    }

    this.isLoading = false;
  }

  async OnDownloadClick(item: PlaylistItemDto) {
    this.currentItem = item;
    this.thumbnailUrl = item.thumbnailUrl;
    this.url = item.url;
    this.artist = item.channel;
    this.title = item.title;
    this.$bvModal.show("modal-download");
  }

  async startDownload(
    event: any,
    item: PlaylistItemDto,
    title: string,
    artist: string,
    url: string
  ) {
    item.downloading = true;

    try {
      const user = await this.$authService.mgr.getUser();
      axios.defaults.headers.common["Authorization"] =
        "Bearer " + user!.access_token;

      await axios.post(process.env.VUE_APP_BASE_URL + "/api/music/download", {
        Title: title,
        Artist: artist,
        Url: url,
      });

      EventBus.$emit("addSuccessToast", "Downloaded '" + title + "'");
      item.downloaded = true;
    } catch (e) {
      EventBus.$emit("addAlertToast", "Failed to download");
    }

    item.downloading = false;
  }
}
</script>

<style lang="scss"></style>
