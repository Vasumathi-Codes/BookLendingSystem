using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class LendingRecordCreateDto
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        public int BookId { get; set; }

        [Range(1, 30, ErrorMessage = "DaysToBorrow must be between 1 and 30.")]
        public int DaysToBorrow { get; set; } = 7;
    }
}
