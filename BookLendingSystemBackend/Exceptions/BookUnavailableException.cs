namespace BookLendingSystem.Exceptions
{
    public class BookUnavailableException : Exception
    {
        public BookUnavailableException(string title)
            : base($"No copies of '{title}' are currently available to borrow.") { }
    }
}
