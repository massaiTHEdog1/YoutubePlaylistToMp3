<template>
  <b-form @submit="onSubmit">
    <loading-pane :isLoading="isLoading" />
    <b-form-group label="Youtube url:" label-for="input-1">
      <b-form-input
        :disabled="isLoading"
        id="input-1"
        v-model="mutableUrl"
        type="url"
        placeholder="Enter video url"
        required
      ></b-form-input>
    </b-form-group>

    <b-form-group label="Artist:" label-for="input-2">
      <b-form-input
        :disabled="isLoading"
        id="input-2"
        v-model="mutableArtist"
        type="text"
        placeholder="Enter artist"
        required
      ></b-form-input>
    </b-form-group>

    <b-form-group label="Title:" label-for="input-3">
      <b-form-input
        :disabled="isLoading"
        id="input-3"
        v-model="mutableTitle"
        type="text"
        placeholder="Enter title"
        required
      ></b-form-input>
    </b-form-group>

    <b-button type="submit" variant="primary" v-if="!isLoading"
      >Download</b-button
    >
  </b-form>
</template>
<script lang="ts">
import LoadingPane from "@/components/LoadingPane.vue";
import { EventBus } from "@/main";
import axios from "axios";
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
  components: {
    LoadingPane,
  },
})
export default class Download extends Vue {
  @Prop() url!: string;
  localUrl = "";
  get mutableUrl(): string {
    return this.url || this.localUrl;
  }
  set mutableUrl(param: string) {
    if (this.url) this.$emit("update:url", param);
    else this.localUrl = param;
  }

  @Prop() artist!: string;
  localArtist = "";
  get mutableArtist(): string {
    return this.artist || this.localArtist;
  }
  set mutableArtist(param: string) {
    if (this.artist) this.$emit("update:artist", param);
    else this.localArtist = param;
  }

  @Prop() title!: string;
  localTitle = "";
  get mutableTitle(): string {
    return this.title || this.localTitle;
  }
  set mutableTitle(param: string) {
    if (this.title) this.$emit("update:title", param);
    else this.localTitle = param;
  }

  isLoading = false;

  async onSubmit(event: Event): Promise<void> {
    this.isLoading = true;
    event.preventDefault();

    try {
      const user = await this.$authService.mgr.getUser();
      axios.defaults.headers.common["Authorization"] =
        "Bearer " + user?.access_token;

      await axios.post(process.env.VUE_APP_BASE_URL + "/api/music/download", {
        Title: this.mutableTitle,
        Artist: this.mutableArtist,
        Url: this.mutableUrl,
      });

      this.mutableUrl = "";
      this.mutableArtist = "";
      this.mutableTitle = "";

      EventBus.$emit("addSuccessToast", "Downloaded !");
    } catch (e) {
      EventBus.$emit("addAlertToast", "Failed to download");
    }

    this.isLoading = false;
  }
}
</script>
