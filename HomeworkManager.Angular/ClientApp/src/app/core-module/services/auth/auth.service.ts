import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../api-client/authorized-api-client/authorized-api-client.service";
import { BehaviorSubject, mergeMap } from "rxjs";
import { UserModel } from "../../../shared-module/models/auth/user-model";
import { AuthenticationResponse } from "../../../shared-module/models/auth/authentication-response";
import { ApiClientService } from "../api-client/api-client.service";
import { AuthenticationRequest } from "../../../shared-module/models/auth/authentication-request";
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
      'Auth/CreateToken',
      authRequest
    ).pipe(
      mergeMap(authResponse => {
        localStorage.setItem('access-token', authResponse.accessToken);
        localStorage.setItem('refresh-token', authResponse.refreshToken);

        return this.authenticate();
      })
    );
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
