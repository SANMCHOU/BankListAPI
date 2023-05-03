using BankListAPI.VsCode.Data;
using System.ComponentModel.DataAnnotations;

namespace BankListAPI.VsCode.Models.User
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
