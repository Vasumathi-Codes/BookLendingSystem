using System.ComponentModel.DataAnnotations;

namespace BookLendingSystem.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be either 'User' or 'Admin'.")]
        public string Role { get; set; } = "User"; 
    }
}
