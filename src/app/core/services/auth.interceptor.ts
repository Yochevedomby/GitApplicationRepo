import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authService.getToken();
    const name=this.authService.getName();
    if (token) {
      const cloned = req.clone({
        setHeaders: {
          'Authorization': `Bearer ${token}`,
          'UserName': name || '',  // שלח את שם המשתמש ב-Header אם קיים
        }
      });
      return next.handle(cloned);
    }
    return next.handle(req);
  }
}
