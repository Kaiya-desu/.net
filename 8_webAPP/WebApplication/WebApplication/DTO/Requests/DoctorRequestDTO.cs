using System.ComponentModel.DataAnnotations;

namespace WebApplication.DTO.Requests
{
    public class DoctorRequestDTO
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "Text is over 100 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Text is over 100 characters.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Text is over 100 characters.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}