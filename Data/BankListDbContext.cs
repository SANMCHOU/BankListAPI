using BankListAPI.VsCode.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankListAPI.VsCode.Data
{
    public class BankListDbContext : IdentityDbContext<ApiUser>
    {
        public BankListDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //created Enity classes Bank, Country & give it all props & then added just to DBSet below using DbSet.
        //it will say these column exists in tables
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new BankConfiguration());
        }
    }
}
