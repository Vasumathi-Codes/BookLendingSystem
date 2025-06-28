import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { RoleService } from '../../services/role.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  templateUrl: './header.html',
  standalone: true,
  imports: [RouterModule, CommonModule],
})
export class Header implements OnInit {
  role: string = '';
  isLoggedIn: boolean = false;

  constructor(private router: Router, private roleService: RoleService) {}

  ngOnInit(): void {
    this.roleService.role$.subscribe((role) => {
      this.role = role;
      this.isLoggedIn = !!role;
    });
  }

  navigateTo(path: string) {
    this.router.navigate([path]);
  }

  navigateToDashboard(): void {
    if (this.role.toLowerCase() === 'admin') {
      this.router.navigate(['/admin-dashboard']);
    } else {
      this.router.navigate(['/user-dashboard']);
    }
  }

  logout() {
    this.roleService.clearRole();
    localStorage.clear(); 
    this.router.navigate(['/login']);
  }
}
