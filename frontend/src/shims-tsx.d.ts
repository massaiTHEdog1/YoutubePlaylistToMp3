import Vue, { VNode } from "vue";

declare global {
  namespace JSX {
    // tslint:disable no-empty-interface
    interface Element extends VNode {}
    // tslint:disable no-empty-interface
    interface ElementClass extends Vue {}
    interface IntrinsicElements {
      [elem: string]: any;
    }
  }
}

import AuthService from "./services/AuthService";

declare module 'vue/types/vue' {
  interface Vue {
    $authService: AuthService;
  }
}