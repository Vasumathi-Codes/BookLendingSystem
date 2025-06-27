namespace BookLendingSystem.Exceptions
{
    public class NoAvailableCopiesException : Exception
    {
        public NoAvailableCopiesException(string bookTitle)
            : base($"No available copies of the book '{bookTitle}' to borrow.") { }
    }
}
