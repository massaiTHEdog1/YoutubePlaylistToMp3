<template>
  <b-container class="mt-2">
    <b-row>
      <b-col>
        <h3>{{ musicsFiltered.length }} musics</h3>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <b-form-group label="Search:" label-for="input-1">
          <b-form-input
            id="input-1"
            type="text"
            style="max-width: 400px"
            v-model="searchText"
            @keyup="search"
          />
        </b-form-group>
      </b-col>
    </b-row>
    <b-row>
      <b-col>
        <music-item
          v-for="music in musicsFiltered"
          v-bind:key="music.title"
          v-bind:music="music"
        />
      </b-col>
    </b-row>
    <loading-pane :isLoading="isLoading" />
  </b-container>
</template>
<script lang="ts">
import { Music } from "@/classes/Music";
import LoadingPane from "@/components/LoadingPane.vue";
import MusicItem from "@/components/MusicItem.vue";
import { EventBus } from "@/main";
import axios from "axios";
import { Component, Vue } from "vue-property-decorator";

@Component({
  components: {
    LoadingPane,
    MusicItem,
  },
})
export default class MusicList extends Vue {
  searchText = "";

  isLoading = false;

  musics: Music[] = [];
  musicsFiltered: Music[] = [];

  async mounted(): Promise<void> {
    this.isLoading = true;

    try {
      const user = await this.$authService.mgr.getUser();
      axios.defaults.headers.common["Authorization"] =
        "Bearer " + user?.access_token;

      var result = await axios.get(
        process.env.VUE_APP_BASE_URL + "/api/music/music-list"
      );

      this.musics = result.data;

      this.musics.forEach((val) =>
        this.musicsFiltered.push(Object.assign({}, val))
      );
    } catch (e) {
      EventBus.$emit(
        "addAlertToast",
        "Loading failed. Please refresh the page."
      );
    }

    this.isLoading = false;
  }

  async search(): Promise<void> {
    if (this.musics.length > 0) {
      this.musicsFiltered = [];

      for (let i = 0; i < this.musics.length; i++) {
        let searchTermFound = true;
        let searchWords = this.searchText.split(" ");

        for (let j = 0; j < searchWords.length; j++) {
          if (
            !this.musics[i].artist.toLowerCase().includes(searchWords[j]) &&
            !this.musics[i].title.toLowerCase().includes(searchWords[j])
          ) {
            searchTermFound = false;
            break;
          }
        }

        if (searchTermFound) this.musicsFiltered.push(this.musics[i]);
      }
    }
  }
}
</script>
