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

  async signIn(
    username: string,
    password: string,
    isRememberMe: boolean = false
  ) {
    try {
      const use = new UserInfo();
      use.userName = username;
      use.passWord = password;
      use.isRememberMe = isRememberMe;

      if (use.userName == 'admin' && use.passWord == 'admin@123') {
        if (use.isRememberMe == true) {
          TokenStorage.setIsLoggedIn(true);
          TokenStorage.setIsReme(true);
          // TokenStorage.saveToken(use);
        } else {
          TokenStorage.clearToken();
          TokenStorage.setIsReme(false);
          TokenStorage.setIsLoggedIn(); // Chỉ lưu trạng thái trong session
        }
        this.router.navigateByUrl('quan-ly-nhan-vien');
        this.commonService.showSuccess('Đăng nhập thành công');
      } else {
        alert('Tài khoản hoặc và mật khẩu không đúng.');
      }
    } catch (err) {
      console.log(err);
      alert('Có lỗi hệ thống!');
      return false;
    }

    return true;
  }

  signOut() {
    TokenStorage.clearToken();
    this.router.navigate(['/login']);
  }

  checkLoggedIn() {
    const IsReme = TokenStorage.getIsReme();
    if (IsReme == true) {
      return this.router.navigateByUrl('quan-ly-nhan-vien');
    } else {
      return this.router.navigateByUrl('login');
    }
  }
}
