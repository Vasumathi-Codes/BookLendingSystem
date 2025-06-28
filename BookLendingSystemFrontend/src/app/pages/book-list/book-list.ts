import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BookReadDto, BookCreateDto, BookUpdateDto } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RoleService } from '../../services/role.service';
import { LendingRecordService } from '../../services/lending-record.service'; 
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-book-list',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './book-list.html',
})
export class BookList implements OnInit {
  books: BookReadDto[] = [];
  pagedBooks: BookReadDto[] = [];

  searchQuery: string = '';
  selectedSort: string = '';
  availabilityFilter: string = '';
  page: number = 1;
  pageSize: number = 6;
  totalPages: number = 1;

  // Add Modal
  addBookModalOpen = false;
  newBook: BookCreateDto = {
    title: '',
    author: '',
    isbn: '',
    totalCopies: 1,
    description: ''
  };

  // Edit Modal
  editBookModalOpen = false;
  selectedBookToEdit: BookReadDto | null = null;
  editBookForm: BookUpdateDto = {
    totalCopies: 1,
    description: ''
  };

  // Borrow Modal
  borrowModalOpen = false;
  selectedBookToBorrow: BookReadDto | null = null;
  borrowDays: number = 7;

  constructor(
    private bookService: BookService,
    private lendingRecordService: LendingRecordService, 
    private toastr: ToastrService,
    private roleService: RoleService
  ) {}

  get isAdmin(): boolean {
    return this.roleService.isAdmin();
  }

  ngOnInit(): void {
    this.fetchBooks();
  }

  fetchBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (res) => {
        this.books = res.items;
        this.applyFilters();
      },
      error: () => {
        this.toastr.error('Failed to fetch books');
      }
    });
  }

  applyFilters(): void {
    let filtered = [...this.books];

    if (this.searchQuery.trim()) {
      filtered = filtered.filter(
        book =>
          book.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          book.author.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }

    if (this.availabilityFilter === 'available') {
      filtered = filtered.filter(book => book.availableCopies > 0);
    } else if (this.availabilityFilter === 'unavailable') {
      filtered = filtered.filter(book => book.availableCopies === 0);
    }

    if (this.selectedSort) {
      filtered.sort((a, b) => {
        if (this.selectedSort === 'availableCopies') {
          return b.availableCopies - a.availableCopies;
        } else if (this.selectedSort === 'title') {
          return a.title.localeCompare(b.title);
        } else if (this.selectedSort === 'author') {
          return a.author.localeCompare(b.author);
        }
        return 0;
      });
    }

    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    this.page = Math.min(this.page, this.totalPages || 1);
    const start = (this.page - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedBooks = filtered.slice(start, end);
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

  deleteBook(id: number): void {
    if (confirm('Are you sure you want to delete this book?')) {
      this.bookService.deleteBook(id).subscribe(() => {
        this.toastr.success('Book deleted');
        this.fetchBooks();
      });
    }
  }

  // ADD Modal Logic
  openAddBookModal() {
    this.addBookModalOpen = true;
  }

  closeAddBookModal() {
    this.addBookModalOpen = false;
    this.resetForm();
  }

  resetForm() {
    this.newBook = {
      title: '',
      author: '',
      isbn: '',
      totalCopies: 1,
      description: ''
    };
  }

  addBook() {
    this.bookService.createBook(this.newBook).subscribe({
      next: () => {
        this.toastr.success('Book added');
        this.closeAddBookModal();
        this.fetchBooks();
      },
      error: () => {
        this.toastr.error('Failed to add book');
      }
    });
  }

  // EDIT Modal Logic
  openEditBookModal(book: BookReadDto) {
    this.selectedBookToEdit = book;
    this.editBookForm = {
      totalCopies: book.totalCopies,
      description: book.description
    };
    this.editBookModalOpen = true;
  }

  closeEditBookModal() {
    this.editBookModalOpen = false;
    this.selectedBookToEdit = null;
    this.editBookForm = {
      totalCopies: 1,
      description: ''
    };
  }

  updateBook(): void {
    if (!this.selectedBookToEdit) return;

    const updatePayload: BookUpdateDto = {
      totalCopies: this.editBookForm.totalCopies,
      description: this.editBookForm.description
    };

    this.bookService.updateBook(this.selectedBookToEdit.id, updatePayload).subscribe({
      next: () => {
        this.toastr.success('Book updated successfully');
        this.closeEditBookModal();
        this.fetchBooks();
      },
      error: (err) => {
        this.toastr.error(err?.error?.error|| 'Failed to update book');
      }
    });
  }

  // BORROW Modal Logic
  openBorrowModal(book: BookReadDto) {
    this.selectedBookToBorrow = book;
    this.borrowDays = 7;
    this.borrowModalOpen = true;
  }

  closeBorrowModal() {
    this.selectedBookToBorrow = null;
    this.borrowModalOpen = false;
    this.borrowDays = 7;
  }

  confirmBorrow() {
    if (!this.selectedBookToBorrow || this.borrowDays < 1 || this.borrowDays > 30) {
      this.toastr.error('Borrowing days must be between 1 and 30');
      return;
    }

    const borrowDto = {
      userId: this.roleService.getUserId(), 
      bookId: this.selectedBookToBorrow.id,
      daysToBorrow: this.borrowDays
    };

    this.lendingRecordService.borrowBook(borrowDto).subscribe({
      next: () => {
        this.toastr.success('Book borrowed successfully');
        this.closeBorrowModal();
        this.fetchBooks();
      },
      error: (err) => {
        this.toastr.error(err?.error?.error|| 'Failed to borrow book');
      }
    });
  }
}
