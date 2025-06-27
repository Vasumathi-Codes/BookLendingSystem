using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class LendingRecordReturnDto
    {
        [Required(ErrorMessage = "LendingRecordId is required.")]
        public int LendingRecordId { get; set; }
    }
}
