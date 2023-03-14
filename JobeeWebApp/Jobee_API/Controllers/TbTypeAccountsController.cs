using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jobee_API.Models;
using Jobee_API.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="ad")]
    public class TbTypeAccountsController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public TbTypeAccountsController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/TbTypeAccounts
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbTypeAccount>>> GetTbTypeAccounts()
        {
            return await _context.TbTypeAccounts.ToListAsync();
        }

        // GET: api/TbTypeAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TbTypeAccount>> GetTbTypeAccount(string id)
        {
            var tbTypeAccount = await _context.TbTypeAccounts.FindAsync(id);

            if (tbTypeAccount == null)
            {
                return NotFound();
            }

            return tbTypeAccount;
        }

        // PUT: api/TbTypeAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTbTypeAccount(string id, TbTypeAccount tbTypeAccount)
        {
            if (id != tbTypeAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(tbTypeAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbTypeAccountExists(id))
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

        // POST: api/TbTypeAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TbTypeAccount>> PostTbTypeAccount(TbTypeAccount tbTypeAccount)
        {
            _context.TbTypeAccounts.Add(tbTypeAccount);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TbTypeAccountExists(tbTypeAccount.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTbTypeAccount", new { id = tbTypeAccount.Id }, tbTypeAccount);
        }

        // DELETE: api/TbTypeAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTbTypeAccount(string id)
        {
            var tbTypeAccount = await _context.TbTypeAccounts.FindAsync(id);
            if (tbTypeAccount == null)
            {
                return NotFound();
            }

            _context.TbTypeAccounts.Remove(tbTypeAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TbTypeAccountExists(string id)
        {
            return _context.TbTypeAccounts.Any(e => e.Id == id);
        }
    }
}
