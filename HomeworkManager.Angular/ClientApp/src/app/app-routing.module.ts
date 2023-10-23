import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import * as auth from './auth-module';
import * as control from './control-module';
import * as homework_manager from './homework-manager-module';
import * as admin from './admin-module';
import { authGuard } from "./core-module";

const routes: Routes = [
  // Auth
  { path: 'login', component: auth.LoginComponent },
  { path: 'register', component: auth.RegisterComponent },

  {
    path: '', component: control.LayoutComponent, children: [
      { path: 'home', component: homework_manager.DashboardComponent, canActivate: [authGuard] },
      { path: 'dashboard', component: homework_manager.DashboardComponent, canActivate: [authGuard] },
      {
        path: 'admin', children: [
          { path: 'users', component: admin.UserListComponent }
        ]
      },
      { path: '**', redirectTo: '/home' }
    ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
