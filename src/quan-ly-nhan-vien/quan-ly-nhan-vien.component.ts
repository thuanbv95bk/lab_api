import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../common/auth/auth.service';

@Component({
  selector: 'app-quan-ly-nhan-vien',
  templateUrl: './quan-ly-nhan-vien.component.html',
  styleUrl: './quan-ly-nhan-vien.component.scss',
})
export class QuanLyNhanVienComponent implements OnInit {
  constructor(public myRoute: Router, private authService: AuthService) {}
  ngOnInit(): void {
    // this.authService.checkLoggedIn();
  }

  goback() {
    this.myRoute.navigateByUrl('');
  }
  signOut() {
    this.authService.signOut();
  }
}
