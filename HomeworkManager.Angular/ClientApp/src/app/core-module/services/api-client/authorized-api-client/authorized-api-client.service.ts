import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { catchError, Observable } from "rxjs";
import { AuthenticationResponse } from "../../../../shared-module/models/auth/authentication-response";
import { RefreshRequest } from "../../../../shared-module/models/auth/refresh-request";
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

  // async get<T>(route: string, onSuccess: (result: T) => void, onError: (error: any) => void) {
  //   await this.httpRequest(() => this.http.get<T>(this.apiUrl + route), onSuccess, onError);
  // }

  // async post<T>(route: string, body: any, onSuccess: (result: T) => void, onError: (error: any) => void) {
  //   await this.httpRequest(() => this.http.post<T>(this.apiUrl + route, body), onSuccess, onError)
  // }

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

  // private async httpRequest<T>(request: () => Observable<T>, onSuccess: (result: T) => void, onError: (error: any) => void) {
  //   request().subscribe({
  //     next: result => onSuccess(result),
  //     error: error => {
  //       if (error.status === 401) {
  //         const accessToken = localStorage.getItem('access-token');
  //         const refreshToken = localStorage.getItem('refresh-token');
  //
  //         if (accessToken !== null && refreshToken !== null) {
  //           this.http.post<AuthenticationResponse>(
  //             this.apiUrl + 'Auth/RefreshToken',
  //             new RefreshRequest(accessToken, refreshToken)
  //           ).subscribe({
  //             next: response => {
  //               localStorage.setItem('access-token', response.accessToken);
  //               localStorage.setItem('refresh-token', response.refreshToken);
  //
  //               request()
  //                 .subscribe({
  //                   next: result => onSuccess(result),
  //                   error: error => {
  //                     if (error.status === 401) {
  //                       this.router.navigate(['/', 'login']);
  //                     } else {
  //                       onError(error);
  //                     }
  //                   }
  //                 })
  //             }
  //           })
  //         } else {
  //           this.router.navigate(['/', 'login']);
  //         }
  //       } else {
  //         onError(error);
  //       }
  //     }
  //   });
  // }
}

/*
req->200->ok
req->401->refresh->200->retry->ok
req->401->refresh->200->retry->error
req->401->error->error
req->error
*/
