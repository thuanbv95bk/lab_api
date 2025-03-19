import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutGridComponent } from '../layout-grid/layout-grid.component';
import { QuanLyNhanVienComponent } from '../quan-ly-nhan-vien/quan-ly-nhan-vien.component';

const routes: Routes = [
  // { path: 'home', component: LayoutComponent,},
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  { path: 'login', component: LayoutGridComponent },
  { path: 'quan-ly-nhan-vien', component: QuanLyNhanVienComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
