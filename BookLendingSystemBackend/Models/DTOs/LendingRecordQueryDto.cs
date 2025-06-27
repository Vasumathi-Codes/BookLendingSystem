namespace BookLendingSystem.DTOs
{
    public class LendingRecordQueryDto
    {
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string? Search { get; set; } = string.Empty;
        public string? SortBy { get; set; } = "BorrowDate"; 
        public string SortOrder { get; set; } = "desc"; 
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
