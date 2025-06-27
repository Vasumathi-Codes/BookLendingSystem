namespace BookLendingSystem.Exceptions
{
    public class DuplicateUsernameException : Exception
    {
        public DuplicateUsernameException(string username)
            : base($"Username '{username}' already exists.")
        {
        }
    }
}
