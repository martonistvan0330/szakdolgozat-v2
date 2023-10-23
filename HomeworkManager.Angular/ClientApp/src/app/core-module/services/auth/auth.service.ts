import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../api-client/authorized-api-client/authorized-api-client.service";
import { BehaviorSubject, mergeMap, of } from "rxjs";
import {
  AuthenticationRequest,
  AuthenticationResponse,
  NewUser,
  RevokeRequest,
  UserModel
} from "../../../shared-module";
import { ApiClientService } from "../api-client/api-client.service";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSource = new BehaviorSubject<UserModel | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  private authApiClient = inject(AuthorizedApiClientService);
  private apiClient = inject(ApiClientService);

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

  authenticate() {
    return this.authApiClient.get<UserModel>('User/Authenticate')
      .pipe(
        map(user => {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        })
      );
  }
}
