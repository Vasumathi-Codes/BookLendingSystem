import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  getRole(): string {
    return localStorage.getItem('role') || '';
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }

  isUser(): boolean {
    return this.getRole() === 'User';
  }

  getUsername(): string {
    return localStorage.getItem('username') || '';
  }

  getUserId(): number {
    const id = localStorage.getItem('id');
    return id ? parseInt(id, 10) : 0;
  }

  clearRole(): void {
    localStorage.removeItem('role');
    localStorage.removeItem('username');
  }
}
