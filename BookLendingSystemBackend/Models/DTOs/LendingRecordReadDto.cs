namespace BookLendingSystem.DTOs
{
    public class LendingRecordReadDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public DateTime DueDate { get; set; }
        public bool IsOverdue => ReturnDate == null && DateTime.UtcNow > DueDate;
    }
}
