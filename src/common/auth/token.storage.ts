import { UserInfo } from '../../model/app-model';

export class TokenStorage {
  public static readonly JWT_TOKEN = 'JWT_TOKEN';
  public static readonly USER_NAME = 'username';
  public static readonly PASS_WORD = 'password';
  public static readonly ISLOGGEDIN = 'loggedIn';
  public static readonly ISREME = 'isreme';
  public static saveToken(user: UserInfo) {
    localStorage.setItem(this.JWT_TOKEN, JSON.stringify(user));
  }

  public static getToken() {
    return localStorage.getItem(this.JWT_TOKEN);
  }
  public static setIsReme(isRememberMe: boolean) {
    if (isRememberMe == true) {
      localStorage.setItem(this.ISREME, 'true');
    } else {
      localStorage.setItem(this.ISREME, 'false');
    }
  }
  public static getIsReme() {
    const IsReme = localStorage.getItem(this.ISREME);
    if (IsReme == 'true') {
      return true;
    } else {
      return false;
    }
  }
  public static isLoggedIn() {
    return localStorage.getItem(this.JWT_TOKEN) != null;
  }

  public static setIsLoggedIn(isRememberMe: boolean = false) {
    localStorage.setItem(this.ISLOGGEDIN, 'true');
    if (isRememberMe == false) {
      sessionStorage.setItem(this.ISLOGGEDIN, 'true');
    }
  }
  public static getIsLoggedIn() {
    if (
      sessionStorage.getItem(this.ISLOGGEDIN) == 'true' ||
      localStorage.getItem(this.ISLOGGEDIN) == 'true'
    ) {
      return true;
    }
    return false;
  }

  public static clearToken() {
    localStorage.removeItem(this.JWT_TOKEN);
    localStorage.removeItem(this.ISLOGGEDIN);
    sessionStorage.removeItem(this.ISLOGGEDIN);
    localStorage.removeItem(this.ISREME);
  }
  public static checkLoggedIn() {
    return TokenStorage.getIsLoggedIn();
  }
}
