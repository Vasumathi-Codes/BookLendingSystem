using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Author name can't exceed 100 characters.")]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1.")]
        public int TotalCopies { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
    }
}
