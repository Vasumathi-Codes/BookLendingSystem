import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/book.service';
import { LendingRecordService } from '../../services/lending-record.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admindashboard',
  imports: [RouterModule],
  templateUrl: './admindashboard.html',
  styleUrl: './admindashboard.css'
})

export class AdminDashboard implements OnInit {
  totalBooks = 0;
  availableBooks = 0;
  returnedCount = 0;
  notReturnedCount = 0;
  overdueCount = 0;
  totalBorrowRecords = 0;

  constructor(
    private bookService: BookService,
    private lendingService: LendingRecordService
  ) {}

  ngOnInit(): void {
    this.fetchBooks();
    this.fetchLendingStats();
  }

  fetchBooks(): void {
    this.bookService.getAllBooks().subscribe(res => {
      const books = res.items;
      this.totalBooks = res.totalCount;
      this.availableBooks = books.filter(b => b.availableCopies > 0).length;
    });
  }

  fetchLendingStats(): void {
    const dto = { pageSize: 1000 };
    this.lendingService.getAllLendingHistory(dto).subscribe(res => {
      const records = res.items;

      this.totalBorrowRecords = records.length;
      this.returnedCount = records.filter(r => r.returnDate).length;
      this.notReturnedCount = records.filter(r => !r.returnDate).length;
      this.overdueCount = records.filter(r => r.isOverdue).length;
    });
  }
}
