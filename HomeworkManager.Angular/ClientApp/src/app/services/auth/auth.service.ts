import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../api-client/authorized-api-client/authorized-api-client.service";
import { BehaviorSubject, mergeMap, of } from "rxjs";
import {
  AuthenticationRequest,
  AuthenticationResponse,
  EmailConfirmationRequest,
  NewUser,
  PasswordRecoveryRequest,
  RevokeRequest,
  Role,
  UserModel
} from "../../shared-module";
import { ApiClientService } from "../api-client/api-client.service";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSource = new BehaviorSubject<UserModel | null>(null);
  private authApiClient = inject(AuthorizedApiClientService);
  private apiClient = inject(ApiClientService);
  currentUser$ = this.currentUserSource.asObservable();

  authenticate() {
    return this.authApiClient.get<UserModel>('User/Authenticate')
      .pipe(
        map(user => {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        })
      );
  }

  login(authRequest: AuthenticationRequest) {
    return this.apiClient.post<AuthenticationResponse>(
      'Auth/Login',
      authRequest
    ).pipe(
      mergeMap(authResponse => {
        localStorage.setItem('access-token', authResponse.accessToken);
        localStorage.setItem('refresh-token', authResponse.refreshToken);

        return this.authenticate();
      })
    );
  }

  register(user: NewUser) {
    return this.apiClient.post<AuthenticationResponse>(
      'Auth/Register',
      user
    ).pipe(
      mergeMap(authResponse => {
        localStorage.setItem('access-token', authResponse.accessToken);
        localStorage.setItem('refresh-token', authResponse.refreshToken);

        return this.authenticate();
      })
    );
  }

  confirmEmail(token: string) {
    return this.apiClient.post<boolean>('Auth/ConfirmEmail', new EmailConfirmationRequest(token));
  }

  logout() {
    const accessToken = localStorage.getItem('access-token');
    const refreshToken = localStorage.getItem('refresh-token');

    if (accessToken !== null && refreshToken !== null) {
      return this.authApiClient.post<boolean>(
        'Auth/Logout',
        new RevokeRequest(accessToken, refreshToken)
      ).pipe(
        map((success) => {
          if (success) {
            localStorage.removeItem('user');
            localStorage.removeItem('access-token');
            localStorage.removeItem('refresh-token');
          }
        })
      );
    } else {
      return of();
    }
  }

  resendConfirmation() {
    return this.authApiClient.patch<boolean>('Auth/ResendConfirmation', null);
  }

  recoverPassword(passwordRecoveryRequest: PasswordRecoveryRequest) {
    return this.apiClient.post<void>('Auth/PasswordRecovery', passwordRecoveryRequest);
  }

  hasRole(roles: Role[]) {
    return this.currentUser$.pipe(
      map(user => {
        if (!user) {
          return false;
        }

        for (const role of user.roles) {
          if (roles.includes(role.roleId)) {
            return true;
          }
        }

        return false;
      })
    )
  }

  userHasRole(user: UserModel, roles: Role[]) {
    if (!user) {
      return false;
    }

    for (const role of user.roles) {
      if (roles.includes(role.roleId)) {
        return true;
      }
    }

    return false;
  }
}
