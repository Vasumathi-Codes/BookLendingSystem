import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserCreateDto, UserLoginDto, UserReadDto } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = 'http://localhost:5000/api/user';

  constructor(private http: HttpClient) {}

  register(user: UserCreateDto): Observable<UserReadDto> {
    return this.http.post<UserReadDto>(`${this.baseUrl}/create`, user);
  }

  login(user: UserLoginDto): Observable<UserReadDto> {
    return this.http.post<UserReadDto>(`${this.baseUrl}/login`, user);
  }

  setHeaders(user: UserReadDto) {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser(): UserReadDto | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  getAuthHeaders(): HttpHeaders {
    const user = this.getUser();
    return user
      ? new HttpHeaders({
          'X-User-Role': user.role,
          'X-User-Name': user.name,
        })
      : new HttpHeaders();
  }

  logout() {
    localStorage.removeItem('user');
  }
}
