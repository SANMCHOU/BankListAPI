using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankListAPI.VsCode.Data;
using BankListAPI.VsCode.Contracts;
using AutoMapper;
using BankListAPI.VsCode.Models.Bank;

namespace BankListAPI.VsCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBanksRepository _bankRepository;
        private readonly IMapper _mapper;

        public BanksController(IBanksRepository banksRepository, IMapper mapper)
        {
            _bankRepository = banksRepository;
            _mapper = mapper;
        }

        // GET: api/Banks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankDto>>> GetBanks()
        {
          if (_bankRepository == null)
          {
              return NotFound();
          }
            var banks = await _bankRepository.GetAllAsync();
            return Ok(_mapper.Map<List<BankDto>>(banks));
        }

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankDto>> GetBank(int id)
        {
          if (_bankRepository == null)
          {
              return NotFound();
          }
            var bank = await _bankRepository.GetAsync(id);

            if (bank == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BankDto>(bank));
        }

        // PUT: api/Banks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBank(int id, BankDto bankDto)
        {
            if (id != bankDto.Id)
            {
                return BadRequest();
            }

            var banks = await _bankRepository.GetAsync(id);
            if (banks == null)
            {
                return NotFound();
            }

            _mapper.Map(bankDto, banks);
            try
            {
                await _bankRepository.UpdateAsync(banks);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BankExists(id))
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

        // POST: api/Banks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateBankDto>> PostBank(CreateBankDto createBankDto)
        {
          if (_bankRepository == null)
          {
              return Problem("_bankRepository is null.");
          }
           var bank = _mapper.Map<Bank>(createBankDto);
           await _bankRepository.AddAsync(bank);

           return CreatedAtAction("GetBank", new { id = bank.Id }, bank);
        }

        // DELETE: api/Banks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            if (_bankRepository == null)
            {
                return NotFound();
            }
            var bank = await _bankRepository.GetAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            await _bankRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> BankExists(int id)
        {
            return await _bankRepository.Exists(id);
        }
    }
}
