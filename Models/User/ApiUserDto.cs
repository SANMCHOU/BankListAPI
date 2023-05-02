using BankListAPI.VsCode.Data;
using System.ComponentModel.DataAnnotations;

namespace BankListAPI.VsCode.Models.User
{
    public class ApiUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
