import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationResponse } from "../models/auth/authentication-response";
import { RefreshRequest } from "../models/auth/refresh-request";
import { Observable } from "rxjs";

@Injectable()
export class SuperHttpClient {
  private readonly http: HttpClient;
  private readonly baseApiUrl: string;
  private readonly apiUrl: string;
  private readonly router: Router;

  constructor(http: HttpClient, @Inject('API_URL') baseApiUrl: string, router: Router) {
    this.http = http;
    this.baseApiUrl = baseApiUrl;
    this.apiUrl = baseApiUrl + '/api';
    this.router = router;
  }

  async get<T>(route: string, onSuccess: (result: T) => void, onError: (error: any) => void) {
    await this.httpRequest(() => this.http.get<T>(this.apiUrl + route), onSuccess, onError);
  }

  async post<T>(route: string, body: any, onSuccess: (result: T) => void, onError: (error: any) => void) {
    await this.httpRequest(() => this.http.post<T>(this.apiUrl + route, body), onSuccess, onError)
  }

  private async httpRequest<T>(request: () => Observable<T>, onSuccess: (result: T) => void, onError: (error: any) => void) {
    request().subscribe({
      next: result => onSuccess(result),
      error: error => {
        if (error.status === 401) {
          const accessToken = localStorage.getItem('access-token');
          const refreshToken = localStorage.getItem('refresh-token');

          if (accessToken !== null && refreshToken !== null) {
            this.http.post<AuthenticationResponse>(
              this.baseApiUrl + 'Auth/RefreshToken',
              new RefreshRequest(accessToken, refreshToken)
            ).subscribe({
              next: response => {
                localStorage.setItem('access-token', response.accessToken);
                localStorage.setItem('refresh-token', response.refreshToken);

                request()
                  .subscribe({
                    next: result => onSuccess(result),
                    error: error => {
                      if (error.status === 401) {
                        this.router.navigate(['/', 'login']);
                      } else {
                        onError(error);
                      }
                    }
                  })
              }
            })
          } else {
            this.router.navigate(['/', 'login']);
          }
        } else {
          onError(error);
        }
      }
    });
  }
}
