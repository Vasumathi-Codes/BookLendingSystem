using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class BookUpdateDto
    {
        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1.")]
        public int? TotalCopies { get; set; }
    }
}
