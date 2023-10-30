import { CanActivateFn, Router } from '@angular/router';
import { inject } from "@angular/core";
import { AuthService } from "../../../services";
import { map } from "rxjs/operators";
import { catchError, mergeMap, switchMap } from "rxjs";
import { Role } from "../../../shared-module";
import { NavigationItems } from "../navigation-items";
import { SnackBarService } from "../../snack-bar/snack-bar.service";

export class AuthGuard {
  static requireAuthenticated: CanActivateFn = (_route, _state) => {
    const authService = inject(AuthService);
    const snackBarService = inject(SnackBarService);

    return this.authenticateUser(authService)
      .pipe(
        mergeMap(() => {
          return authService.currentUser$.pipe(
            catchError((error, caught) => {
              snackBarService.error('Authentication Failed', error.error)
              throw error;
            }),
            map(user => {
              return !!user;
            })
          )
        })
      );
  }

  static requireAnyRole(roles: Role[]): CanActivateFn {
    return (_route, _state) => {
      const authService = inject(AuthService);

      return this.authenticateUser(authService)
        .pipe(
          switchMap(() => {
            return authService.hasRole(roles);
          })
        );
    }
  }

  static requireUserOrAdmin: CanActivateFn = (route, _state) => {
    const authService = inject(AuthService);

    return this.authenticateUser(authService)
      .pipe(
        mergeMap(() => {
          return authService.currentUser$.pipe(
            map(user => {
              if (!user) {
                return false;
              }

              const userId = route.paramMap.get('id');

              if (!userId) {
                return false;
              }

              if (user.userId === userId) {
                return true;
              }

              return authService.userHasRole(user, [Role.ADMINISTRATOR]);
            })
          )
        })
      );
  }

  private static authenticateUser(authService: AuthService) {
    const router = inject(Router);

    return authService.authenticate()
      .pipe(
        catchError(error => {
          router.navigate([NavigationItems.login.navigationUrl]).then(_ => {
          });
          throw error;
        })
      );
  }
}