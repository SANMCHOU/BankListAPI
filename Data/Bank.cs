using System.ComponentModel.DataAnnotations.Schema;

namespace BankListAPI.VsCode.Data
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }

        [ForeignKey(nameof(CountryId))]
        public int CountryId { get; set; }
        public Country CountryName { get; set; }
    }
}
