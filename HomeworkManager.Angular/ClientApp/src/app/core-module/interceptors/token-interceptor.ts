import { Injectable } from "@angular/core";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = localStorage.getItem('access-token');
    if (accessToken !== null) {
      const modifiedRequest = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + accessToken)
      });
      return next.handle(modifiedRequest);
    }

    return next.handle(request);
  }
}