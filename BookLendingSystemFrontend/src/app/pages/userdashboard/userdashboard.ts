import { Component, OnInit } from '@angular/core';
import { LendingRecordService } from '../../services/lending-record.service';
import { RoleService } from '../../services/role.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-userdashboard',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './userdashboard.html',
  styleUrls: ['./userdashboard.css']
})
export class UserDashboard implements OnInit {
  userId = 0;

  totalBorrowed = 0;
  returnedCount = 0;
  notReturnedCount = 0;
  overdueCount = 0;

  constructor(
    private lendingService: LendingRecordService,
    private roleService: RoleService
  ) {
    this.userId = this.roleService.getUserId();
  }

  ngOnInit(): void {
    this.loadUserStats();
  }

  loadUserStats(): void {
    const dto = {
      userId: this.userId,
      pageSize: 1000
    };

    this.lendingService.getUserBorrowedBooks(dto).subscribe(res => {
      const records = res.items;
      this.totalBorrowed = records.length;
      this.returnedCount = records.filter(r => r.returnDate).length;
      this.notReturnedCount = records.filter(r => !r.returnDate).length;
      this.overdueCount = records.filter(r => r.isOverdue).length;
    });
  }
}
