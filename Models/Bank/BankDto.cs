using System.ComponentModel.DataAnnotations.Schema;

namespace BankListAPI.VsCode.Models.Bank
{
    public class BankDto : BaseBankDto
    {
        public int Id { get; set; }
    }
}