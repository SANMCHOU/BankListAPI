using System.ComponentModel.DataAnnotations;

namespace BankListAPI.VsCode.Models.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
