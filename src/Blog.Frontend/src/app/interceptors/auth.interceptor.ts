import { Injectable } from '@angular/core';

import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const jwt = localStorage.getItem('jwt');
    let exp: number | string | null = localStorage.getItem('exp');
    if(jwt && exp) {
      exp = Number.parseInt(exp);
      if(exp * 1000 >= Date.now()) {
        request = request.clone({
          setHeaders: { 'Authorization': `Bearer ${jwt}`}
        });
      }
      else{
        localStorage.removeItem('auth-token');
      }
    }
    return next.handle(request);
  }
}
