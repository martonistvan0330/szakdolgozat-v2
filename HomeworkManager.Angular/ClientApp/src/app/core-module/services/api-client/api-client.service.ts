import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ApiClientService {
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
    return this.httpRequest(() => this.http.post<T>(this.apiUrl + route, body))
  }

  private httpRequest<T>(request: () => Observable<T>) {
    return request();
  }
}
