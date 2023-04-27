using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using Microsoft.EntityFrameworkCore;

namespace BankListAPI.VsCode.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly BankListDbContext _context;
        public CountryRepository(BankListDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(s => s.Banks).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
