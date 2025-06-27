using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
