import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserReadDto } from '../../models/user.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users',
  imports: [CommonModule],
  templateUrl: './users.html'
})
export class Users implements OnInit {
  users: UserReadDto[] = [];

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe({
      next: (res:UserReadDto[]) => this.users = res,
      error: () => alert('Failed to load users')
    });
  }
}
