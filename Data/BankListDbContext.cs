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
            modelBuilder.Entity<Country>().HasData(
                new Country { 
                    Id = 1, 
                    Name = "India", 
                    ShortName = "IN" 
                },
                new Country { 
                    Id = 2, 
                    Name = "United Kingdom", 
                    ShortName = "UK" 
                }
            );

            modelBuilder.Entity<Bank>().HasData(
                new Bank
                {
                    Id = 1,
                    Name = "HDFC Bank",
                    Address = "Trivandrum Kerala",
                    Rating = 4.5,
                    CountryId = 1
                },
                new Bank
                {
                    Id = 2,
                    Name = "HSBC Bank",
                    Address = "Leamingtion Spa",
                    Rating = 4,
                    CountryId = 2
                },
                new Bank
                {
                     Id = 3,
                     Name = "Barclays",
                     Address = "Leamingtion Spa",
                     Rating = 4.5,
                     CountryId = 2
                },
                new Bank
                {
                    Id = 4,
                    Name = "SBI",
                    Address = "Athani, India",
                    Rating = 4.8,
                    CountryId = 1
                }
            );
        }
    }
}
