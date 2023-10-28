import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { catchError, Observable, switchMap } from "rxjs";
import { AuthenticationResponse, RefreshRequest } from "../../../shared-module";

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

  put<T>(route: string, body: any) {
    return this.httpRequest(() => this.http.put<T>(this.apiUrl + route, body));
  }

  private httpRequest<T>(request: () => Observable<T>) {
    return request().pipe(
      catchError((error, caught) => {
        if (error.status === 401) {
          const accessToken = localStorage.getItem('access-token');
          const refreshToken = localStorage.getItem('refresh-token');

          if (accessToken !== null && refreshToken !== null) {
            return this.http.post<AuthenticationResponse>(
              this.apiUrl + 'Auth/RefreshToken',
              new RefreshRequest(accessToken, refreshToken)
            ).pipe(
              catchError(error => {
                throw error;
              }),
              switchMap(authResponse => {
                localStorage.setItem('access-token', authResponse.accessToken);
                localStorage.setItem('refresh-token', authResponse.refreshToken);

                return caught;
              })
            );
          }
        }
        throw error;
      })
    )
  }
}
