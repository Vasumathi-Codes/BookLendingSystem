namespace BookLendingSystem.Exceptions
{
    public class LendingRecordNotFoundException : Exception
    {
        public LendingRecordNotFoundException(int userId, int bookId)
            : base($"No active lending record found for User ID {userId} and Book ID {bookId}.") { }
    }
}
