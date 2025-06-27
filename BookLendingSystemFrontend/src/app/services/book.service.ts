import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  BookReadDto,
  BookCreateDto,
  BookUpdateDto
} from '../models/book.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private baseUrl = 'http://localhost:5000/api/book';

  constructor(private http: HttpClient) {}

  getAllBooks(params: any = {}): Observable<PagedResult<BookReadDto>> {
    return this.http.get<PagedResult<BookReadDto>>(this.baseUrl, { params });
  }

  getBookById(id: number): Observable<BookReadDto> {
    return this.http.get<BookReadDto>(`${this.baseUrl}/${id}`);
  }

  createBook(book: BookCreateDto): Observable<BookReadDto> {
    return this.http.post<BookReadDto>(`${this.baseUrl}/create`, book);
  }

  updateBook(id: number, book: BookUpdateDto): Observable<BookReadDto> {
    return this.http.put<BookReadDto>(`${this.baseUrl}/update/${id}`, book);
  }

  deleteBook(id: number): Observable<BookReadDto> {
    return this.http.delete<BookReadDto>(`${this.baseUrl}/delete/${id}`);
  }
}
