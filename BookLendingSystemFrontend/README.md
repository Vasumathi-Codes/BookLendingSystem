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

