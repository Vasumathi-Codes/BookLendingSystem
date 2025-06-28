import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../services/book.service';
import { LendingRecordService } from '../../services/lending-record.service';
import { BookReadDto, BookUpdateDto } from '../../models/book.model';
import { RoleService } from '../../services/role.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-book-detail',
  templateUrl: './book-detail.html',
  styleUrl: './book-detail.css',
  imports: [FormsModule, CommonModule]
})
export class BookDetail implements OnInit {
  book!: BookReadDto;
  hasBorrowed = false;
  borrowModalOpen = false;
  borrowDays: number = 7;
  userId = 0;

  // Edit Modal
  editModalOpen = false;
  editBookForm: BookUpdateDto = {
    totalCopies: 1,
    description: ''
  };

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private lendingService: LendingRecordService,
    private roleService: RoleService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.userId = this.roleService.getUserId();
  }

  get isAdmin(): boolean {
    return this.roleService.isAdmin(); 
  }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!;
    this.bookService.getBookById(id).subscribe(book => {
      this.book = book;
      if (!this.isAdmin) {
        this.checkIfUserHasBorrowed(); 
      }
    });
  }

  checkIfUserHasBorrowed(): void {
    this.lendingService.getUserBorrowedBooks({ userId: this.userId, pageSize: 1000 }).subscribe(res => {
      this.hasBorrowed = res.items.some(r => r.bookId === this.book.id && !r.returnDate);
    });
  }

  // Borrow Modal Logic
  openBorrowModal(): void {
    if (this.isAdmin) {
      this.toastr.warning("Admins cannot borrow books.");
      return;
    }
    this.borrowDays = 7;
    this.borrowModalOpen = true;
  }

  closeBorrowModal(): void {
    this.borrowModalOpen = false;
  }

  borrowBook(): void {
    if (this.borrowDays < 1 || this.borrowDays > 30) {
      this.toastr.error('Borrowing days must be between 1 and 30');
      return;
    }

    this.lendingService.borrowBook({
      userId: this.userId,
      bookId: this.book.id,
      daysToBorrow: this.borrowDays
    }).subscribe({
      next: () => {
        this.toastr.success('Book borrowed successfully');
        this.closeBorrowModal();
        this.ngOnInit();
      },
      error: () => {
        this.toastr.error('Failed to borrow book');
      }
    });
  }

  // Edit Modal Logic
  openEditModal(): void {
    this.editBookForm = {
      totalCopies: this.book.totalCopies,
      description: this.book.description
    };
    this.editModalOpen = true;
  }

  closeEditModal(): void {
    this.editModalOpen = false;
  }

  updateBook(): void {
    const updatePayload: BookUpdateDto = {
      totalCopies: this.editBookForm.totalCopies,
      description: this.editBookForm.description
    };

    this.bookService.updateBook(this.book.id, updatePayload).subscribe({
      next: () => {
        this.toastr.success('Book updated successfully');
        this.closeEditModal();
        this.ngOnInit(); // reload updated data
      },
      error: (err) => {
        this.toastr.error(err?.error?.error || 'Failed to update book');
      }
    });
  }

  deleteBook(): void {
  if (!this.isAdmin) return;

  if (confirm('Are you sure you want to delete this book?')) {
    this.bookService.deleteBook(this.book.id).subscribe({
      next: () => {
        this.toastr.success('Book deleted successfully');
        this.router.navigate(['/book-list']);
      },
      error: () => {
        this.toastr.error('Failed to delete the book');
      }
    });
  }
}

}
