namespace BankListAPI.VsCode.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual IList<Bank> Banks { get; set;}
    }
}