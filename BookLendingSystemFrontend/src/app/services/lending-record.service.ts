import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  LendingRecordCreateDto,
  LendingRecordReadDto,
  LendingRecordQueryDto
} from '../models/lending-record.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})
export class LendingRecordService {
  private baseUrl = 'http://localhost:5000/api/lending-records';

  constructor(private http: HttpClient) {}

  borrowBook(dto: LendingRecordCreateDto): Observable<LendingRecordReadDto> {
    return this.http.post<LendingRecordReadDto>(`${this.baseUrl}/borrowbook`, dto);
  }

  returnBook(userId: number, bookId: number): Observable<LendingRecordReadDto> {
    const params = new HttpParams()
      .set('userId', userId.toString())
      .set('bookId', bookId.toString());

    return this.http.put<LendingRecordReadDto>(`${this.baseUrl}/returnbook`, {}, { params });
  }

  getUserBorrowedBooks(dto: LendingRecordQueryDto): Observable<PagedResult<LendingRecordReadDto>> {
    const params = this.buildQueryParams(dto);
    return this.http.get<PagedResult<LendingRecordReadDto>>(`${this.baseUrl}/user/borrowed`, { params });
  }

  getAllLendingHistory(dto: LendingRecordQueryDto): Observable<PagedResult<LendingRecordReadDto>> {
    const params = this.buildQueryParams(dto);
    return this.http.get<PagedResult<LendingRecordReadDto>>(`${this.baseUrl}/history`, { params });
  }

  getOverdueBooks(dto: LendingRecordQueryDto): Observable<PagedResult<LendingRecordReadDto>> {
    const params = this.buildQueryParams(dto);
    return this.http.get<PagedResult<LendingRecordReadDto>>(`${this.baseUrl}/overdue`, { params });
  }

  private buildQueryParams(dto: LendingRecordQueryDto): HttpParams {
    let params = new HttpParams();

    if (dto.userId !== undefined) params = params.set('userId', dto.userId.toString());
    if (dto.bookId !== undefined) params = params.set('bookId', dto.bookId.toString());
    if (dto.search) params = params.set('search', dto.search);
    if (dto.sortBy) params = params.set('sortBy', dto.sortBy);
    if (dto.sortOrder) params = params.set('sortOrder', dto.sortOrder);
    if (dto.page) params = params.set('page', dto.page.toString());
    if (dto.pageSize) params = params.set('pageSize', dto.pageSize.toString());

    return params;
  }
}
