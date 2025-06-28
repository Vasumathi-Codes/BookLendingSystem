import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { BookList } from './pages/book-list/book-list';
import { BorrowHistory } from './pages/borrow-history/borrow-history';
import { AuthGuard } from './guards/auth-guard';
import { AdminDashboard } from './pages/admindashboard/admindashboard';
import { UserDashboard } from './pages/userdashboard/userdashboard';
import { Users } from './pages/users/users';

export const routes: Routes = [
    { path: 'login', component: Login },
    { path: 'register', component: Register },
    { path: 'book-list', component: BookList, canActivate: [AuthGuard] },
    { path: 'borrow-list', component: BorrowHistory, canActivate: [AuthGuard] },
    { path: 'admin-dashboard', component: AdminDashboard, canActivate: [AuthGuard] },
    { path: 'user-dashboard', component: UserDashboard, canActivate: [AuthGuard] },
    {path: 'book-detail/:id',loadComponent: () => import('./pages/book-detail/book-detail').then(m => m.BookDetail)},
    { path: 'users', component: Users},
    { path: '**', redirectTo: 'login' }
];
