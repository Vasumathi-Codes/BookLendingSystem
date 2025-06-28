import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RoleService {
  private roleSubject = new BehaviorSubject<string>(localStorage.getItem('role') || '');
  role$ = this.roleSubject.asObservable();

  private usernameSubject = new BehaviorSubject<string>(localStorage.getItem('username') || '');
  username$ = this.usernameSubject.asObservable();

  setRole(role: string, username: string, id: number): void {
    localStorage.setItem('role', role);
    localStorage.setItem('username', username);
    localStorage.setItem('id', id.toString());

    this.roleSubject.next(role);
    this.usernameSubject.next(username);
  }

  clearRole(): void {
    localStorage.removeItem('role');
    localStorage.removeItem('username');
    localStorage.removeItem('id');
    this.roleSubject.next('');
    this.usernameSubject.next('');
  }

  getUserId(): number {
    const id = localStorage.getItem('id');
    return id ? parseInt(id, 10) : 0;
  }

  getCurrentRole(): string {
    return this.roleSubject.value;
  }

  getCurrentUsername(): string {
    return this.usernameSubject.value;
  }

  isAdmin(): boolean {
    return this.getCurrentRole() === 'Admin';
  }

  isUser(): boolean {
    return this.getCurrentRole() === 'User';
  }
}
