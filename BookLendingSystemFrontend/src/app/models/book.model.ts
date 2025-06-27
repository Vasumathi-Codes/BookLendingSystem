
export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  totalCopies: number;
  availableCopies: number;
  description: string;
}

export interface BookCreateDto {
  title: string;
  author: string;
  isbn: string;
  totalCopies: number;
  description: string;
}

export interface BookUpdateDto {
  totalCopies?: number;
  description?: string;
}

export interface BookReadDto {
  id: number;
  title: string;
  author: string;
  isbn: string;
  totalCopies: number;
  availableCopies: number;
  description: string;
}
