import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserReadDto } from '../models/user.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:5000/api/User'; 

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<UserReadDto[]> {
    return this.http.get<UserReadDto[]>(this.baseUrl);
  }
}
