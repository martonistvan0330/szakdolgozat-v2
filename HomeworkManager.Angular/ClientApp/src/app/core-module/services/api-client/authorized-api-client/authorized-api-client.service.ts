import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { catchError, Observable } from "rxjs";
import { AuthenticationResponse, RefreshRequest } from "../../../../shared-module";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AuthorizedApiClientService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiUrl: string;

  constructor(@Inject('API_URL') apiUrl: string) {
    this.apiUrl = apiUrl;
  }

  get<T>(route: string) {
    return this.httpRequest(() => this.http.get<T>(this.apiUrl + route));
  }

  post<T>(route: string, body: any) {
    return this.httpRequest(() => this.http.post<T>(this.apiUrl + route, body));
  }

  private httpRequest<T>(request: () => Observable<T>) {
    return request().pipe(
      catchError((error, caught) => {
        if (error.status === 401) {
          const accessToken = localStorage.getItem('access-token');
          const refreshToken = localStorage.getItem('refresh-token');

          if (accessToken !== null && refreshToken !== null) {
            this.http.post<AuthenticationResponse>(
              this.apiUrl + 'Auth/RefreshToken',
              new RefreshRequest(accessToken, refreshToken)
            ).pipe(
              catchError(error => {
                throw error;
              }),
              map(authResponse => {
                localStorage.setItem('access-token', authResponse.accessToken);
                localStorage.setItem('refresh-token', authResponse.refreshToken);

                return caught;
              })
            );
          } else {
            throw error;
          }
        }
        throw error;
      })
    )
  }
}
