namespace BookLendingSystem.DTOs
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
