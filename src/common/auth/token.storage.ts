import { UserInfo } from '../../model/app-model';

export class TokenStorage {
  public static readonly JWT_TOKEN = 'JWT_TOKEN';
  public static readonly USER_NAME = 'username';
  public static readonly PASS_WORD = 'password';
  public static readonly ISLOGGEDIN = 'loggedIn';
  public static readonly ISREME = 'isreme';

  public static saveToken(userName: string, passWord: string) {
    localStorage.setItem(this.USER_NAME, userName);
    localStorage.setItem(this.PASS_WORD, passWord);
  }

  public static getToken() {
    const _token = new UserInfo();
    _token.userName = localStorage.getItem(this.USER_NAME);
    _token.passWord = localStorage.getItem(this.PASS_WORD);

    return _token;
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
    if (isRememberMe == false) {
      sessionStorage.setItem(this.ISLOGGEDIN, 'true');
      localStorage.removeItem(this.ISLOGGEDIN);
    } else {
      localStorage.setItem(this.ISLOGGEDIN, 'true');
      sessionStorage.removeItem(this.ISLOGGEDIN);
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
    localStorage.removeItem(this.ISREME);
    localStorage.removeItem(this.USER_NAME);
    localStorage.removeItem(this.PASS_WORD);
    sessionStorage.removeItem(this.ISLOGGEDIN);
  }

  public static checkLoggedIn() {
    const isReme = TokenStorage.getIsReme();
    const isLoggedIn = TokenStorage.getIsLoggedIn();
    const userInfor = TokenStorage.getToken();
    console.log('checkLoggedIn');
    console.log(isReme);
    console.log(isLoggedIn);

    if (isReme == true || isLoggedIn == true) {
      return 'quan-ly-nhan-vien';
    }
    if (userInfor.userName == 'admin' && userInfor.passWord == 'admin@123') {
      return 'quan-ly-nhan-vien';
    }
    if (isReme == false || isLoggedIn == false) {
      return 'login';
    }
    return '';
  }
}
