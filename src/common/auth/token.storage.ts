import { UserInfo } from '../../model/app-model';

export class TokenStorage {
  public static readonly JWT_TOKEN = 'JWT_TOKEN';
  public static readonly USER_NAME = 'username';
  public static readonly PASS_WORD = 'password';
  public static readonly ISLOGGEDIN = 'loggedIn';

  public static saveToken(user: UserInfo) {
    localStorage.setItem(this.JWT_TOKEN, JSON.stringify(user));
  }

  public static getToken() {
    return localStorage.getItem(this.JWT_TOKEN);
  }

  public static isLoggedIn() {
    return localStorage.getItem(this.JWT_TOKEN) != null;
  }

  public static setIsLoggedIn(isRememberMe: boolean = false) {
    if (isRememberMe == true) {
      return localStorage.setItem(this.ISLOGGEDIN, 'true');
    }
    return sessionStorage.setItem(this.ISLOGGEDIN, 'true');
  }
  public static getIsLoggedIn() {
    if (
      sessionStorage.getItem(this.ISLOGGEDIN) ||
      localStorage.getItem(this.ISLOGGEDIN)
    ) {
      return true;
    }
    return false;
  }

  public static clearToken() {
    localStorage.removeItem(this.JWT_TOKEN);
    localStorage.removeItem(this.ISLOGGEDIN);
    sessionStorage.removeItem(this.ISLOGGEDIN);
  }
  public static checkLoggedIn() {
    return TokenStorage.getIsLoggedIn();
  }
}
