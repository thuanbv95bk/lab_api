import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutGridComponent } from '../layout-grid/layout-grid.component';
import { QuanLyNhanVienComponent } from '../quan-ly-nhan-vien/quan-ly-nhan-vien.component';
import { TokenStorage } from '../common/auth/token.storage';

const routes: Routes = [
  // { path: 'home', component: LayoutComponent,},
  {
    path: '',
    redirectTo:
      TokenStorage.getIsLoggedIn() == true ? 'quan-ly-nhan-vien' : 'login',
    pathMatch: 'full',
  },
  { path: 'login', component: LayoutGridComponent },
  { path: 'quan-ly-nhan-vien', component: QuanLyNhanVienComponent },
  // { path: '', redirectTo: 'login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
