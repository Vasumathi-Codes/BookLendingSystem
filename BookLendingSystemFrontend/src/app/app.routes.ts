import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { BookList } from './pages/book-list/book-list';
import { BorrowHistory } from './pages/borrow-history/borrow-history';

export const routes: Routes = [
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'book-list', component: BookList },
    { path: 'borrow-list', component: BorrowHistory },
];
