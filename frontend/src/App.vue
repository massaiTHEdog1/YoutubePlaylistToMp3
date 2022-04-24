<template>
  <b-container fluid>
    <b-row style="background-color: #212121">
      <router-link to="/playlist" exact class="nav-item">Playlist</router-link>

      <router-link to="/download" class="nav-item">Download</router-link>

      <router-link to="/music-list" class="nav-item">Music list</router-link>
    </b-row>
    <b-row>
      <b-col>
        <keep-alive>
          <router-view />
        </keep-alive>
      </b-col>
    </b-row>
  </b-container>
</template>

<script lang="ts">
import { EventBus } from "@/main";
import { User } from "oidc-client";
import { Component, Vue } from "vue-property-decorator";

@Component({
  components: {},
})
export default class App extends Vue {
  user: User | null = null;

  mounted(): void {
    EventBus.$on("addAlertToast", this.addAlertToast);
    EventBus.$on("addSuccessToast", this.addSuccessToast);
  }

  async addAlertToast(text: string): Promise<void> {
    this.$bvToast.toast(text, {
      title: "Error",
      variant: "danger",
      solid: true,
      appendToast: true,
      toaster: "b-toaster-bottom-right",
    });
  }

  async addSuccessToast(text: string): Promise<void> {
    this.$bvToast.toast(text, {
      title: "Success",
      variant: "success",
      solid: true,
      appendToast: true,
      toaster: "b-toaster-bottom-right",
    });
  }
}
</script>

<style lang="scss" scoped>
@import "bootstrap/scss/_functions.scss";
@import "bootstrap/scss/_variables.scss";
@import "bootstrap/scss/_mixins.scss";

@include media-breakpoint-up(xs) {
  .nav-item {
    width: 180px;
  }
}
@include media-breakpoint-down(xs) {
  .nav-item {
    width: 100%;
  }
}

.nav-item {
  height: 40px;
  line-height: 40px;
  text-align: center;
  color: white;
  text-decoration: none;
}

.nav-item:hover {
  background-color: #383838;
}

.router-link-active {
  font-weight: bold;
  background-color: #383838;
}

.router-link-active:hover {
  background-color: #4c4c4c;
}
</style>
