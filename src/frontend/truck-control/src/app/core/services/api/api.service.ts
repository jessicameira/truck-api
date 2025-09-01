// src/app/core/services/api/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, switchMap } from 'rxjs';
import { environment } from '../../../../environments/environments';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private auth: AuthService
) { }

    private ensureAuthenticated(): Observable<boolean> {
  if (this.auth.isAuthenticated()) {
    return of(true);
  }

  return this.auth.generateToken().pipe(
    switchMap(() => of(true)),
    catchError(error => {
      console.error('Erro ao gerar token:', error);
      return of(false);
    })
  );
}

  get<T>(endpoint: string): Observable<T> {
    return this.ensureAuthenticated().pipe(
      switchMap(() => this.http.get<T>(`${this.apiUrl}/${endpoint}`))
    );
  }

  post<T>(endpoint: string, data: any): Observable<T> {
    return this.ensureAuthenticated().pipe(
      switchMap(() => this.http.post<T>(`${this.apiUrl}/${endpoint}`, data))
    );
  }

  put<T>(endpoint: string, data: any): Observable<T> {
    return this.ensureAuthenticated().pipe(
      switchMap(() => this.http.put<T>(`${this.apiUrl}/${endpoint}`, data))
    );
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.ensureAuthenticated().pipe(
      switchMap(() => this.http.delete<T>(`${this.apiUrl}/${endpoint}`))
    );
  }
}