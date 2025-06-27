namespace BookLendingSystem.Exceptions
{
    public class DuplicateBorrowingException : Exception
    {
        public DuplicateBorrowingException(string userName, string bookTitle)
            : base($"User '{userName}' has already borrowed the book '{bookTitle}' and has not returned it yet.") { }
    }
}
