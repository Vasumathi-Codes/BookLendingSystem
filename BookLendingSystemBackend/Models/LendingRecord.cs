namespace BookLendingSystem.Models;

public class LendingRecord: BaseEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }
}
