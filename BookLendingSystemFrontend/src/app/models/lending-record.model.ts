export interface LendingRecordReadDto {
  id: number;
  userId: number;
  userName: string;
  bookId: number;
  bookTitle: string;
  borrowDate: string;
  returnDate: string | null;
  dueDate: string;
  isOverdue: boolean;
}

export interface LendingRecordCreateDto {
  userId: number;
  bookId: number;
  daysToBorrow: number;
}

export interface LendingRecordQueryDto {
  userId?: number;
  bookId?: number;
  search?: string;
  sortBy?: string;
  sortOrder?: string;
  page?: number;
  pageSize?: number;
}

interface LendingRecordWithStatus extends LendingRecordReadDto {
  status: 'Returned' | 'Overdue' | 'In Progress';
}
