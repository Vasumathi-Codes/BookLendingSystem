import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const role = localStorage.getItem('role');
    const username = localStorage.getItem('username');

    if (role && username) {
      const cloned = req.clone({
        setHeaders: {
          'X-User-Role': role,
          'X-User-Name': username
        }
      });
      return next.handle(cloned);
    }

    return next.handle(req);
  }
}
