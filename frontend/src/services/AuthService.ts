import axios from "axios";
import Oidc, { User, UserManager, UserManagerSettings } from "oidc-client";

export default class AuthService {
  mgr: UserManager;

  constructor() {
    const config: UserManagerSettings = {
      userStore: new Oidc.WebStorageStateStore({ store: localStorage }),
      authority: process.env.VUE_APP_SSO_AUTHORITY,
      client_id: process.env.VUE_APP_SSO_CLIENT_ID,
      redirect_uri: `${window.location.origin}/login-callback`,
      response_type: "code",
      scope: "openid roles",
      post_logout_redirect_uri: window.location.origin,
      automaticSilentRenew: true,
      silent_redirect_uri: `${window.location.origin}/login-silent-callback`,
    };
    this.mgr = new Oidc.UserManager(config);
  }
}
