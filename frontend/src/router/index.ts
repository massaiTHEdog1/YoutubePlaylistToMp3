import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
	{
		path: "/",
		name: "Index",
		//lazy-loading
		component: () => import("../views/Playlist.vue"),
	},
	{
		path: "/download",
		name: "Download",
		//lazy-loading
		component: () => import("../views/Download.vue"),
	},
	{
		path: "/music-list",
		name: "MusicList",
		//lazy-loading
		component: () => import("../views/MusicList.vue"),
	},
];

const router = new VueRouter({
	mode: "history",
	base: process.env.BASE_URL,
	routes,
});

export default router;
