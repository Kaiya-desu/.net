using System.ComponentModel.DataAnnotations;

namespace WebApplication.DTO.Requests
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Login is required")]
        [MaxLength(30, ErrorMessage = "Text is over 30 characters.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(30, ErrorMessage = "Text is over 30 characters.")]
        public string Password { get; set; }
    }
}