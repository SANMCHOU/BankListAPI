using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankListAPI.VsCode.Models.Bank
{
    public class BaseBankDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public double? Rating { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int CountryId { get; set;}
    }
}
