import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { TokenStorage } from './token.storage';
import { CommonService } from '../../service/common.service';
import { UserInfo } from '../../model/app-model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    public commonService: CommonService,
    protected httpClient: HttpClient,
    private router: Router
  ) {}

  goPageHome() {
    this.router.navigateByUrl('quan-ly-nhan-vien');
  }

  async signIn(
    userName: string,
    passWord: string,
    isRememberMe: boolean = false,
    dieuHuongUrl: string
  ) {
    try {
      const use = new UserInfo();
      use.userName = userName;
      use.passWord = passWord;
      use.isRememberMe = isRememberMe;

      if (use.userName == 'admin' && use.passWord == 'admin@123') {
        TokenStorage.setIsLoggedIn(use.isRememberMe);

        if (use.isRememberMe == true) {
          TokenStorage.setIsReme(true);
          TokenStorage.saveToken(use.userName, use.passWord);
        } else {
          TokenStorage.clearToken();
          TokenStorage.setIsReme(false);
        }

        this.goPageHome();
        this.commonService.showSuccess('Đăng nhập thành công');
      } else {
        alert('Tài khoản hoặc và mật khẩu không đúng.');
      }
    } catch (err) {
      alert(err);
      return false;
    }

    return true;
  }

  signOut() {
    TokenStorage.clearToken();
    this.router.navigate(['/login']);
  }

  checkLoggedIn() {
    const isReme = TokenStorage.getIsReme();
    const isLoggedIn = TokenStorage.getIsLoggedIn();
    const userInfor = TokenStorage.getToken();
    console.log('checkLoggedIn');
    console.log(isReme);
    console.log(isLoggedIn);

    if (isReme == true) {
      this.goPageHome();
      return;
    }
    if (userInfor.userName == 'admin' && userInfor.passWord == 'admin@123') {
      this.goPageHome();
      return;
    }
    if (isReme == false || isLoggedIn == false) {
      this.router.navigateByUrl('login');
      return;
    }
  }
}
