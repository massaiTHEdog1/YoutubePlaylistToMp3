import AuthService from "@/services/AuthService";
import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Index",
    redirect: { name: "Playlist" },
  },
  {
    path: "/playlist",
    name: "Playlist",
    //lazy-loading
    component: () => import("../views/Playlist.vue"),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/download",
    name: "Download",
    //lazy-loading
    component: () => import("../views/Download.vue"),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/music-list",
    name: "MusicList",
    //lazy-loading
    component: () => import("../views/MusicList.vue"),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/login",
    name: "Login",
    component: () => import("../views/Authentication/Login.vue"),
  },
  {
    path: "/login-callback",
    name: "LoginCallback",
    component: () => import("../views/Authentication/LoginCallback.vue"),
  },
  {
    path: "/login-silent-callback",
    name: "LoginSilentCallback",
    component: () => import("../views/Authentication/LoginSilentCallback.vue"),
  },
];

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

router.beforeEach(async (to, from, next) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  if (requiresAuth) {
    const user = await (
      Vue.prototype.$authService as AuthService
    ).mgr.getUser();
    if (!user) {
      next("/login");
    } else {
      if (user.expired) {
        await (Vue.prototype.$authService as AuthService).mgr.signinSilent();
      }
      next();
    }
  } else {
    next();
  }
});

export default router;
