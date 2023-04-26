using AutoMapper;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.Bank;
using BankListAPI.VsCode.Models.Country;

namespace BankListAPI.VsCode.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig() {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Bank, BankDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
        }
    }
}
