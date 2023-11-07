import { CanActivateFn, Router } from '@angular/router';
import { inject } from "@angular/core";
import { AuthService } from "../../../services";
import { map } from "rxjs/operators";
import { catchError, Observable, of, switchMap } from "rxjs";
import { Role } from "../../../shared-module";
import { NavigationItems } from "../navigation-items";
import { SnackBarService } from "../../snack-bar/snack-bar.service";
import { UserService } from "../../../user-module";
import { CourseService, GroupService } from "../../../course-module";

export class AuthGuard {
  static requireAuthenticated(): CanActivateFn {
    return (_route, _state) => {
      const authService = inject(AuthService);
      const router = inject(Router);
      const snackBarService = inject(SnackBarService);

      return authService.authenticate()
        .pipe(
          catchError(() => {
            router.navigate([NavigationItems.login.navigationUrl])
              .then(success => {
                if (success) {
                  snackBarService.error('Authentication Failed', 'Please log in');
                }
              });
            return of();
          }),
          switchMap(() => {
            return authService.currentUser$
              .pipe(
                map(user => {
                  return !!user;
                })
              )
          })
        );
    }
  }

  static requireAnyRole(roles: Role[]): CanActivateFn {
    return (_route, _state) => {
      const authService = inject(AuthService);

      return authService.hasRole(roles);
    }
  }

  static requireUserExists(): CanActivateFn {
    return (route, _state) => {
      const userService = inject(UserService);

      const userId = route.paramMap.get('userId');

      if (!userId) {
        return of(false);
      }

      return userService.getUser(userId)
        .pipe(
          catchError(() => {
            return of(null);
          }),
          map(userModel => {
            return !!userModel;
          })
        );
    }
  }

  static requireIsUser(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$
          .pipe(
            map(user => {
              if (!user) {
                return false;
              }

              const userId = route.paramMap.get('userId');

              if (!userId) {
                return false;
              }

              return user.userId === userId
            })
          )
      );
    }
  }

  static requireCourseExists(): CanActivateFn {
    return (route, _state) => {
      const courseService = inject(CourseService);
      const groupService = inject(GroupService);

      const courseId = route.paramMap.get('courseId');

      if (!courseId) {
        return of(false);
      }

      groupService.courseId = parseInt(courseId);

      return courseService.existsCourse(parseInt(courseId));
    }
  }

  static requireIsInCourse(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);
      const courseService = inject(CourseService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$.pipe(
          switchMap(user => {
            if (!user) {
              return of(false);
            }

            const courseId = route.paramMap.get('courseId');

            if (!courseId) {
              return of(false);
            }

            return courseService.isInCourse(parseInt(courseId));
          })
        )
      );
    }
  }

  static requireIsCourseCreator(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);
      const courseService = inject(CourseService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$.pipe(
          switchMap(user => {
            if (!user) {
              return of(false);
            }

            const courseId = route.paramMap.get('courseId');

            if (!courseId) {
              return of(false);
            }

            return courseService.isCreator(parseInt(courseId));
          })
        )
      );
    }
  }

  static requireIsCourseTeacher(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);
      const courseService = inject(CourseService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$.pipe(
          switchMap(user => {
            if (!user) {
              return of(false);
            }

            const courseId = route.paramMap.get('courseId');

            if (!courseId) {
              return of(false);
            }

            return courseService.isTeacher(parseInt(courseId));
          })
        )
      );
    }
  }

  static requireGroupExists(): CanActivateFn {
    return (route, _state) => {
      const groupService = inject(GroupService);

      const groupName = route.paramMap.get('groupName');

      if (!groupName) {
        return of(false);
      }

      return groupService.existsGroup(groupName);
    }
  }

  static requireIsInGroup(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);
      const groupService = inject(GroupService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$.pipe(
          switchMap(user => {
            if (!user) {
              return of(false);
            }

            const groupName = route.paramMap.get('groupName');

            if (!groupName) {
              return of(false);
            }

            return groupService.isInGroup(groupName);
          })
        )
      );
    }
  }

  static requireIsGroupCreator(): CanActivateFn {
    return (route, _state) => {
      const authService = inject(AuthService);
      const groupService = inject(GroupService);

      return this.IsAdministrator(
        authService,
        authService.currentUser$.pipe(
          switchMap(user => {
            if (!user) {
              return of(false);
            }

            const groupName = route.paramMap.get('groupName');

            if (!groupName) {
              return of(false);
            }

            return groupService.isCreator(groupName);
          })
        )
      );
    }
  }

  private static IsAdministrator(authService: AuthService, observable: Observable<boolean>) {
    return authService.hasRole([Role.ADMINISTRATOR])
      .pipe(
        switchMap(isAdmin => {
          if (isAdmin) {
            return of(true);
          }

          return observable;
        })
      );
  }
}