import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import * as auth from './auth-module';
import * as control from './control-module';
import * as course from './course-module';
import * as homework_manager from './homework-manager-module';
import * as user from './user-module';
import { AuthGuard, NavigationItems } from "./core-module";

const routes: Routes = [
  // Auth
  { path: NavigationItems.login.routerUrlPattern, component: auth.LoginComponent },
  { path: NavigationItems.register.routerUrlPattern, component: auth.RegisterComponent },
  { path: NavigationItems.emailConfirmation.routerUrlPattern, component: auth.EmailConfirmationComponent },
  { path: NavigationItems.passwordRecovery.routerUrlPattern, component: auth.PasswordRecoveryComponent },
  { path: NavigationItems.passwordReset.routerUrlPattern, component: auth.PasswordResetComponent },

  // App
  {
    path: '', component: control.LayoutComponent, canActivate: [AuthGuard.requireAuthenticated()], children: [
      {
        path: NavigationItems.home.routerUrlPattern,
        component: homework_manager.DashboardComponent
      },
      {
        path: 'dashboard',
        component: homework_manager.DashboardComponent
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
        canActivate: [AuthGuard.requireUserExists, AuthGuard.requireIsUser()]
      },

      // Course
      {
        path: NavigationItems.courseList.routerUrlPattern,
        component: course.CourseListComponent
      },
      {
        path: NavigationItems.courseEdit.routerUrlPattern,
        component: course.CourseEditComponent,
        resolve: {
          course: course.courseDetailsResolver
        },
        canActivate: [AuthGuard.requireCourseExists(), AuthGuard.requireAnyRole(NavigationItems.courseEdit.roles), AuthGuard.requireIsCourseCreator()]
      },
      {
        path: NavigationItems.courseCreate.routerUrlPattern,
        component: course.CourseCreateComponent,
        canActivate: [AuthGuard.requireAnyRole(NavigationItems.courseCreate.roles)]
      },
      {
        path: NavigationItems.courseDetails.routerUrlPattern,
        component: course.CourseDetailsComponent,
        resolve: {
          course: course.courseDetailsResolver
        },
        canActivate: [AuthGuard.requireCourseExists(), AuthGuard.requireIsInCourse()],
        children: [
          {
            path: NavigationItems.groupCreate.routerUrlPattern,
            component: course.GroupCreateComponent,
            canActivate: [AuthGuard.requireAnyRole(NavigationItems.groupCreate.roles), AuthGuard.requireIsCourseTeacher]
          },
          {
            path: NavigationItems.groupEdit.routerUrlPattern,
            component: course.GroupEditComponent,
            resolve: {
              group: course.groupDetailsResolver
            },
            canActivate: [AuthGuard.requireAnyRole(NavigationItems.groupEdit.roles), AuthGuard.requireIsGroupCreator()]
          },
          {
            path: NavigationItems.groupDetails.routerUrlPattern,
            component: course.GroupDetailsComponent,
            resolve: {
              group: course.groupDetailsResolver
            },
            canActivate: [AuthGuard.requireGroupExists(), AuthGuard.requireIsInGroup()]
          },

          { path: '**', redirectTo: `${NavigationItems.groupDetails.navigationUrl}/General` }
        ]
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
