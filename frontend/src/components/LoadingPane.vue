<template>
	<div v-if="isLoading" class="loadingPane">
		<b-spinner variant="primary"></b-spinner>
	</div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component
export default class LoadingPane extends Vue {
	@Prop() private isLoading! : boolean;
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
	.loadingPane
	{
		position: absolute;
		left: 0;
		top: 0;
		right: 0;
		bottom: 0;
		background-color: rgba(0,0,0,0.1);
		display:flex;
		justify-content:center;
		align-items:center;
	}
</style>
