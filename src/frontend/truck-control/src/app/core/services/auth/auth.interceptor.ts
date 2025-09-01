// src/app/core/services/auth/auth.interceptor.ts
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

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (this.isApiRequest(request) && !this.isAuthRequest(request)) {
      const token = this.authService.getToken();
      
      if (token) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
      }
    }

    return next.handle(request);
  }

  private isApiRequest(request: HttpRequest<any>): boolean {
    return request.url.includes('/api/');
  }

  private isAuthRequest(request: HttpRequest<any>): boolean {
    return request.url.includes('/auth/');
  }
}