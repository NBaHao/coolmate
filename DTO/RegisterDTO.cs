using System.ComponentModel.DataAnnotations;

namespace CoolMate.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
