using AutoMapper;
using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;
using Microsoft.EntityFrameworkCore;

namespace BankListAPI.VsCode.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly BankListDbContext _context;
        private readonly IMapper _mapper;
        public CountryRepository(BankListDbContext context, IMapper mapper) : base(context, mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(s => s.Banks).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
