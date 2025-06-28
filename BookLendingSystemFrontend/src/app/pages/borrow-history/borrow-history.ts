import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LendingRecordService } from '../../services/lending-record.service';
import { LendingRecordReadDto } from '../../models/lending-record.model';
import { RoleService } from '../../services/role.service';
import { ToastrService } from 'ngx-toastr';

type RecordStatus = 'Returned' | 'Overdue' | 'In Progress';

interface LendingRecordWithStatus extends LendingRecordReadDto {
  status: RecordStatus;
}

@Component({
  standalone: true,
  selector: 'app-borrow-history',
  imports: [CommonModule, FormsModule],
  templateUrl: './borrow-history.html'
})
export class BorrowHistory implements OnInit {
  lendingRecords: LendingRecordWithStatus[] = [];
  filteredRecords: LendingRecordWithStatus[] = [];
  pagedRecords: LendingRecordWithStatus[] = [];

  isAdmin = false;
  userId = 0;
  sortOrder: 'asc' | 'desc' = 'asc';

  searchQuery = '';
  selectedStatus = '';
  selectedSort = '';
  page = 1;
  pageSize = 5;
  totalPages = 1;

  constructor(
    private lendingService: LendingRecordService,
    private roleService: RoleService,
    private toastr: ToastrService
  ) {
    this.userId = this.roleService.getUserId();
    this.isAdmin = this.roleService.isAdmin();
  }

  ngOnInit(): void {
    this.fetchRecords();
  }

  fetchRecords(): void {
    const dto = {
      userId: this.isAdmin ? undefined : this.userId,
      pageSize: 1000
    };

    const obs = this.isAdmin
      ? this.lendingService.getAllLendingHistory(dto)
      : this.lendingService.getUserBorrowedBooks(dto);

    obs.subscribe({
      next: (res) => {
        this.lendingRecords = res.items.map(record => ({
          ...record,
          status: this.getStatus(record)
        }));
        this.applyFilters();
      },
      error: () => this.toastr.error('Failed to fetch lending records')
    });
  }

  getStatus(record: LendingRecordReadDto): RecordStatus {
    if (record.returnDate) return 'Returned';
    if (record.isOverdue) return 'Overdue';
    return 'In Progress';
  }

  applyFilters(): void {
    let records = [...this.lendingRecords];

    if (this.searchQuery.trim()) {
      records = records.filter(r =>
        r.bookTitle.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }

    if (this.selectedStatus) {
      records = records.filter(r => r.status === this.selectedStatus);
    }

    if (this.selectedSort === 'borrowDate') {
      records.sort((a, b) => new Date(b.borrowDate).getTime() - new Date(a.borrowDate).getTime());
    } else if (this.selectedSort === 'dueDate') {
      records.sort((a, b) => new Date(b.dueDate).getTime() - new Date(a.dueDate).getTime());
    } else if (this.selectedSort === 'status') {
      records.sort((a, b) => a.status.localeCompare(b.status));
    }

    if (this.selectedSort) {
      records.sort((a, b) => {
        let comparison = 0;

        if (this.selectedSort === 'borrowDate') {
          comparison = new Date(a.borrowDate).getTime() - new Date(b.borrowDate).getTime();
        } else if (this.selectedSort === 'dueDate') {
          comparison = new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime();
        } else if (this.selectedSort === 'status') {
          comparison = a.status.localeCompare(b.status);
        }

        return this.sortOrder === 'asc' ? comparison : -comparison;
      });
    }


    this.filteredRecords = records;
    this.totalPages = Math.ceil(records.length / this.pageSize);
    this.page = Math.min(this.page, this.totalPages || 1);
    const start = (this.page - 1) * this.pageSize;
    this.pagedRecords = records.slice(start, start + this.pageSize);
  }

  changePageSize(): void {
    this.page = 1;
    this.applyFilters();
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.applyFilters();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.applyFilters();
    }
  }

  returnBook(record: LendingRecordWithStatus): void {
    this.lendingService.returnBook(record.userId, record.bookId).subscribe({
      next: () => {
        this.toastr.success('Book returned successfully!');
        this.fetchRecords();
      },
      error: () => {
        this.toastr.error('Failed to return book');
      }
    });
  }

  getOverdueDays(dueDate: string): number {
    const due = new Date(dueDate);
    const today = new Date();
    const diff = today.getTime() - due.getTime();
    return Math.ceil(diff / (1000 * 3600 * 24));
  }

}
