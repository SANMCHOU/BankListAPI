using BankListAPI.VsCode.Contracts;
using BankListAPI.VsCode.Data;

namespace BankListAPI.VsCode.Repository
{
    public class BanksRepository : GenericRepository<Bank>, IBanksRepository
    {
        public BanksRepository(BankListDbContext context) : base(context)
        {
        }
    }
}
