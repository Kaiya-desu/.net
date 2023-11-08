using System.ComponentModel.DataAnnotations;

namespace WebApplication.DTO.Requests
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(30, ErrorMessage = "Text is over 30 characters.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(30, ErrorMessage = "Text is over 30 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Text is over 100 characters.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}