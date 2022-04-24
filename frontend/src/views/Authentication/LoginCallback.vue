<template>
  <p>Redirecting...</p>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component({})
export default class LoginCallback extends Vue {
  async mounted(): Promise<void> {
    try {
      await this.$authService.mgr.signinRedirectCallback();
    } catch (e) {
      //
    }

    this.$router.push({ name: "Index" });
  }
}
</script>
