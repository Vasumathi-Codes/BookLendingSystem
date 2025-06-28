import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { RoleService } from '../../../services/role.service';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
})
export class Login {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toastr: ToastrService,
    private roleService: RoleService
  ) {
    this.loginForm = this.fb.group({
      name: ['', Validators.required],
      role: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.auth.login(this.loginForm.value).subscribe({
        next: (res) => {
          this.toastr.success('Login successful!');
          this.auth.setHeaders(res);
          this.roleService.setRole(res.role, res.name, res.id);
          localStorage.setItem('username', res.name);
          localStorage.setItem('role', res.role);
          localStorage.setItem('id', res.id.toString());

          setTimeout(() => {
            this.router.navigate([`/${res.role.toLowerCase()}-dashboard`]);
          }, 500);
        },
        error: (err) => {
          this.toastr.error(err?.error?.error || 'Login failed. Try again.');
          console.log(err?.error?.error);
        }
      });
    }
  }
}
