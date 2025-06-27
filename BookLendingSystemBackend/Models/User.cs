namespace BookLendingSystem.Models
{
    public class User: BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public ICollection<LendingRecord>? LendingRecords { get; set; }
    }

}
