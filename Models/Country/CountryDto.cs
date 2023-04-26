using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.Bank;

namespace BankListAPI.VsCode.Models.Country
{
    public class CountryDto : GetCountryDto
    {
        public List<BankDto> Banks { get; set; }

    }
}
