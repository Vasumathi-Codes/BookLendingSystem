namespace BookLendingSystem.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(int bookId)
            : base($"Book with ID {bookId} was not found.") { }
    }
}
