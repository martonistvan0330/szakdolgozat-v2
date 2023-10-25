import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import * as auth from './auth-module';
import * as control from './control-module';
import * as homework_manager from './homework-manager-module';
import * as user from './user-module';
import { AuthGuard, NavigationItems } from "./core-module";

const routes: Routes = [
  // Auth
  { path: 'login', component: auth.LoginComponent },
  { path: 'register', component: auth.RegisterComponent },

  // App
  {
    path: '', component: control.LayoutComponent, children: [
      {
        path: NavigationItems.home.routerUrlPattern,
        component: homework_manager.DashboardComponent,
        canActivate: [AuthGuard.requireAuthenticated]
      },
      {
        path: 'dashboard',
        component: homework_manager.DashboardComponent,
        canActivate: [AuthGuard.requireAuthenticated]
      },

      // User
      {
        path: NavigationItems.userList.routerUrlPattern,
        component: user.UserListComponent,
        canActivate: [AuthGuard.requireAnyRole(NavigationItems.userList.roles)]
      },
      {
        path: NavigationItems.userDetail.routerUrlPattern,
        component: user.UserDetailComponent,
        resolve: {
          user: user.userResolver
        },
        canActivate: [AuthGuard.requireUserOrAdmin]
      },
      { path: '**', redirectTo: NavigationItems.home.navigationUrl }
    ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
