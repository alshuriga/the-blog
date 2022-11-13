import { Injectable } from '@angular/core';
import { tap, catchError, of } from 'rxjs';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ServerValidationService } from '../services/server-validation.service';

@Injectable()
export class ServerErrorsInterceptor implements HttpInterceptor {

  constructor(private router: Router, private valdation: ServerValidationService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler,): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(catchError((err) => {
      if (err.error instanceof ErrorEvent) {

      }
      else {
        switch (err.status) {
          case 401: {
            this.router.navigate(['signin']);
            break;
          }
          case 403:
            {
              this.router.navigate(['access-denied']);
              break;
            }
          case 422:
            {
              this.HandleBackendValidators(err);
              break;
            }
          case 400:
            {
              alert(err.error.message)
              break;
            }
          default: {
            break
          }
        }

      }
      return of(err);
    }));
  }

  public HandleBackendValidators(err: HttpErrorResponse) {
    const errors: any = {};
    for (const key in err.error) {
      if (Object.prototype.hasOwnProperty.call(err.error, key)) {
        errors[key] = err.error[key];
      }
      this.valdation.addServerErrors(errors);
    }
  }
}
