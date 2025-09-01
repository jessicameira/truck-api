// src/app/core/services/auth/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environments';

export interface TokenResponse {
  token: string;
  expiresIn: number;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'authToken';
  private tokenSubject = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
    this.loadStoredToken();
  }

  private loadStoredToken(): void {
    const token = localStorage.getItem(this.tokenKey);
    this.tokenSubject.next(token);
  }

  generateToken(): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(`${this.apiUrl}/generate-token`, {}).pipe(
      tap(response => this.storeToken(response.token))
    );
  }

  private storeToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    this.tokenSubject.next(token);
  }

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  removeToken(): void {
    localStorage.removeItem(this.tokenKey);
    this.tokenSubject.next(null);
  }
}