<template>
  <div class="music">
    <h5>{{ music.title }}</h5>
    <p>{{ music.artist }}</p>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { Music } from "@/classes/Music";

@Component
export default class MusicItem extends Vue {
  @Prop() music!: Music;
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
.music h5 {
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2; /* number of lines to show */
  -webkit-box-orient: vertical;
  margin: 0;
}

.music p {
  color: rgb(170, 170, 170);
  margin: 0;
}

.music:hover {
  background-color: #272727;
}
</style>
