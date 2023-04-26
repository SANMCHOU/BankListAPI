using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Models.Country;
using AutoMapper;

namespace BankListAPI.VsCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly BankListDbContext _context;
        private readonly IMapper _mapper;

        public CountriesController(BankListDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }

            var countires = await _context.Countries.ToListAsync();
            //Automapper
            var records = _mapper.Map<List<GetCountryDto>>(countires);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
          if (_context.Countries == null)
          {
              return NotFound();
          }

            //Default EF syntax
            //var country = await _context.Countries.FindAsync(id);
            ////Automapper: Include banks for which the id matched the bank IDs
            var country = await _context.Countries.Include(s => s.Banks).FirstOrDefaultAsync(q => q.Id == id);

            if (country == null)
            {
                return NotFound();
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

            //_context.Entry(country).State = EntityState.Modified;
            //Automapper
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            _mapper.Map<UpdateCountryDto>(country);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {
            //overposting attacks:
            //1. Adding Data Transfer Object.
            //var country = new Country()
            //{
            //    Name = createCountry.Name,
            //    ShortName = createCountry.ShortName,
            //};

            //2. Set automapper to protect from overposting attacks 
            var country = _mapper.Map<Country>(createCountry);

            if (_context.Countries == null)
            {
              return Problem("Entity set 'BankListDbContext.Countries'  is null.");
            }
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (_context.Countries == null)
            {
                return NotFound();
            }
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
