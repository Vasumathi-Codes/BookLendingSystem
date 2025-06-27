namespace BookLendingSystem.Exceptions
{
    public class BookAlreadyBorrowedException : Exception
    {
        public BookAlreadyBorrowedException(string bookTitle)
            : base($"You have already borrowed '{bookTitle}'. Please return it before borrowing again.") { }
    }
}
