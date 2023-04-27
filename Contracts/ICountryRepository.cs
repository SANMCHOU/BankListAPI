using BankListAPI.VsCode.Data;

namespace BankListAPI.VsCode.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
