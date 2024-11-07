// src/app/core/services/auth.service.ts
import { HttpClient } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login } from 'src/app/models/login';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }
  private apiUrl = "https://localhost:7183/api/Login/login";

  login(user: Login): Observable<any> {
    //const body = { username, password };
    return this.http.post(this.apiUrl, user);
  }


  saveToken(token: string, name: string): void {
    localStorage.setItem('jwtToken', token); // שמירת הטוקן ב-localStorage
    localStorage.setItem('name', name);
  }

  getToken(): string | null {
    return localStorage.getItem('jwtToken');

  }
  getName(): string | null {
    return localStorage.getItem('name');

  }
  logout(): void {
    localStorage.removeItem('jwtToken'); // הסרת הטוקן ביציאה
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('jwtToken');
  }




}
