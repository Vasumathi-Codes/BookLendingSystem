# BookLendingSystem

## Overview

The Book Lending System is a full-stack application backend built using ASP.NET Core. It allows users to borrow and return books, while admins manage the book inventory and track lending activity.
---
## Features

### User Roles
- Two roles: `User` and `Admin`
- Role and user name are passed via HTTP headers
- Custom middleware injects roles as claims for authorization

### Admin Capabilities
- Add, edit, and delete books
- View list of all books and their availability
- View full borrowing history of all users

### User Capabilities
- View available books
- Borrow a book if copies are available
- View currently borrowed books
- Return borrowed books

### Borrowing Logic
- Borrowing decreases available copies
- Returning increases available copies
- Prevents double borrowing of the same book
- Return deadline enforced (e.g., 14 days)
- Overdue tracking implemented

---

## Technologies Used

- ASP.NET Core 7
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Log4Net
- Swagger for API testing

---
## API Endpoints
| Method | Endpoint                | Description                 | Role  |
| ------ | ----------------------- | --------------------------- | ----- |
| GET    | `/api/book`             | Get all books               | All   |
| GET    | `/api/book/{id}`        | Get a book by ID            | All   |
| POST   | `/api/book/create`      | Add new book                | Admin |
| PUT    | `/api/book/update/{id}` | Update book details         | Admin |
| DELETE | `/api/book/delete/{id}` | Delete a book (soft delete) | Admin |

| Method | Endpoint                | Description           | Role  |
| ------ | ----------------------- | --------------------- | ----- |
| GET    | `/api/user`             | Get list of all users | Admin |
| GET    | `/api/user/{id}`        | Get user by ID        | Admin |
| POST   | `/api/user/create`      | Create a new user     | All   |
| POST   | `/api/user/login`       | Simulate user login   | All   |
| PUT    | `/api/user/update/{id}` | Update user           | Admin |
| DELETE | `/api/user/delete/{id}` | Delete user           | Admin |

| Method | Endpoint                             | Description                           | Role  |
| ------ | ------------------------------------ | ------------------------------------- | ----- |
| POST   | `/api/lending-records/borrowbook`    | Borrow a book                         | User  |
| PUT    | `/api/lending-records/returnbook`    | Return a book                         | User  |
| GET    | `/api/lending-records/user/borrowed` | View books currently borrowed by user | User  |
| GET    | `/api/lending-records/history`       | View borrowing history (all users)    | Admin |
| GET    | `/api/lending-records/overdue`       | View overdue books                    | Admin |


## API Endpoints
ExceptionHandlingMiddleware: Logs unhandled exceptions with appropriate status codes.
RoleInjectionMiddleware: Injects X-User-Name and X-User-Role from request headers into the HTTP context for role-based authorization.

--------------------------------------------------------------------------------------------------------------------------------

# Book Lending System - Frontend

This is the Angular frontend for the Book Lending System application. The system enables two types of users – Admin and User – to manage book inventory, lending activities, and return tracking.

## Features

### User Roles
- Two roles: Admin and User
- Role selection is available on login (no authentication implemented)
- Role-specific access control for features

### Admin Functionalities
- Add new books with:
  - Title
  - Author
  - ISBN
  - Total copies
  - Short description
- Edit existing book details
- Delete books from the catalog
- View list of all books along with their availability status
- View all lending records from all users
- Export lending records to CSV or PDF
- Mobile-responsive layout for all views

### User Functionalities
- View the list of available books
- View detailed information about each book
- Borrow a book (only if copies are available)
- Return borrowed books
- View borrowing history with borrow, due, return dates, and status

### Book Details Page
- Displays:
  - Book title
  - Author
  - ISBN
  - Description
  - Total and available copies
- Borrow button is disabled if no copies are available

### Borrowing Logic
- Available copies decrease upon borrowing
- Available copies increase upon return
- Prevents users from borrowing the same book twice without returning it

### Overdue Tracking (Challenging Requirement)
- Borrowed books are assigned a due date
- Status is calculated based on return status and current date:
  - In Progress
  - Returned
  - Overdue
- Overdue books display number of days overdue
- Visual highlight of overdue records with color indication

### Filters and Sorting
- Filter by:
  - Book title (search)
  - Lending status (In Progress, Returned, Overdue)
- Sort by:
  - Borrow date
  - Due date
  - Status
- Supports ascending and descending order toggle
- Pagination with selectable page size (5, 10, 20)

### Responsive Design
- Fully responsive layout using Tailwind CSS
- Download buttons repositioned below headings for mobile view
- Navbar adapts for mobile screens

### Export Features
- Export borrowing history as CSV
- Export borrowing history as formatted PDF
- Export supports filters and pagination context

## Technology Stack
- Angular 20 (standalone components)
- Tailwind CSS
- RxJS for data streams
- ngx-toastr for toast notifications
- html2pdf and custom CSV service for export functionality

