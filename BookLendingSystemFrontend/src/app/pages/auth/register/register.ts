import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.html',
})
export class Register {
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      role: ['User', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.auth.register(this.registerForm.value).subscribe({
        next: (res) => {
          this.toastr.success('Registered successfully!');
          this.auth.setHeaders(res);
          setTimeout(() => {
            this.router.navigate([`/${res.role.toLowerCase()}`]);
          }, 1000);
        },
        error: (err) => {
          this.toastr.error(err?.error?.error || 'Registration failed. Try again.');
          console.error(err?.error?.error);
        }
      });
    }
  }
}
