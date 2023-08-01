using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankListAPI.VsCode.Data;
using AutoMapper;
using BankListAPI.VsCode.Core.Contracts;
using BankListAPI.VsCode.Core.Models.Country;
using BankListAPI.VsCode.Core.Models;
using BankListAPI.VsCode.Core.Exceptions;
using BankListAPI.VsCode.Data.Data;

namespace BankListAPI.VsCode.Controllers
{
   // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;

        public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
          if (_countriesRepository == null)
          {
              return NotFound();
          }

            //var countires = await _countriesRepository.GetAllAsync();
            //var records = _mapper.Map<List<GetCountryDto>>(countires);
            var countries = await _countriesRepository.GetAllAsync();
            return Ok(countries);
        }

        // GET: api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters 
            queryParameters)
        {
            if (_countriesRepository == null)
            {
                return NotFound();
            }

            var pagedCountriesResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountriesResult);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
          if (_countriesRepository == null)
          {
              return NotFound();
          }

            //var country = await _countriesRepository.GetDetails(id);

            //if (country == null)
            //{
            //    throw new NotFoundExceptions(nameof(GetCountry), id);
            //}
            //var record = _mapper.Map<CountryDto>(country);
            var record = await _countriesRepository.GetDetails(id);

            return record;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
            {
                return BadRequest();
            }

            //var country = await _countriesRepository.GetAsync(id);
            //if (country == null)
            //{
            //    throw new NotFoundExceptions(nameof(PutCountry), id);
            //}
            //_mapper.Map(updateCountry,country);

            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateCountryDto>> PostCountry(CreateCountryDto createCountry)
        {
            //var country = _mapper.Map<Country>(createCountry);

            //if (_countriesRepository == null)
            //{
            //  return Problem("Entity set 'BankListDbContext.Countries'  is null.");
            //}

            var country = await _countriesRepository.AddAsync<CreateCountryDto,GetCountryDto>(createCountry);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            //if (_countriesRepository == null)
            //{
            //    return NotFound();
            //}
            //var country = await _countriesRepository.GetAsync(id);
            //if (country == null)
            //{
            //    return NotFound();
            //}

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await (_countriesRepository.Exists(id));
        }
    }
}
