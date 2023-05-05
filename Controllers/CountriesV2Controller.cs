using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankListAPI.VsCode.Data;
using AutoMapper;
using Microsoft.AspNetCore.OData.Query;
using BankListAPI.VsCode.Core.Contracts;
using BankListAPI.VsCode.Core.Models.Country;
using BankListAPI.VsCode.Core.Exceptions;
using BankListAPI.VsCode.Data.Data;

namespace BankListAPI.VsCode.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CountriesV2Controller : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;

        public CountriesV2Controller(ICountriesRepository countriesRepository, IMapper mapper)
        {
            _countriesRepository = countriesRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
          if (_countriesRepository == null)
          {
              return NotFound();
          }

            var countires = await _countriesRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCountryDto>>(countires);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
          if (_countriesRepository == null)
          {
              return NotFound();
          }

            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                throw new NotFoundExceptions(nameof(GetCountry), id);
            }
            var record = _mapper.Map<CountryDto>(country);

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

            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundExceptions(nameof(PutCountry), id);
            }
            _mapper.Map(updateCountry,country);

            try
            {
                await _countriesRepository.UpdateAsync(country);
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
            var country = _mapper.Map<Country>(createCountry);

            if (_countriesRepository == null)
            {
              return Problem("Entity set 'BankListDbContext.Countries'  is null.");
            }

            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_countriesRepository == null)
            {
                return NotFound();
            }
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await (_countriesRepository.Exists(id));
        }
    }
}
