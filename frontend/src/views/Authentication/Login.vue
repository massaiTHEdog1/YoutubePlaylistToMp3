<template>
  <b-button @click="login">Login</b-button>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class Login extends Vue {
  async login(): Promise<void> {
    await this.$authService.mgr.signinRedirect();
  }
}
</script>
