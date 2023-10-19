import { CanActivateFn, Router } from '@angular/router';
import { inject } from "@angular/core";
import { AuthService } from "../../services/auth/auth.service";
import { map } from "rxjs/operators";
import { catchError, mergeMap } from "rxjs";

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);

  return authService.authenticate()
    .pipe(
      catchError(error => {
        router.navigateByUrl('/login');
        throw error;
      }),
      mergeMap(() => {
        return authService.currentUser$.pipe(
          map(user => {
            return !!user;
          })
        )
      })
    );
};
