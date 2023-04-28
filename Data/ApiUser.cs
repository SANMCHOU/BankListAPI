using Microsoft.AspNetCore.Identity;

namespace BankListAPI.VsCode.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
