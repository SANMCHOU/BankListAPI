using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace BankListAPI.VsCode.Data.Configurations
{
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.HasData(
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
