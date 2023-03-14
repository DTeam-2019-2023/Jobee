using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public AwardsController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/Educations
        [HttpGet("listAwards")]
        public async Task<ActionResult<List<Award>>> GetAwardsByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            var dbAward = _context.Awards.Where(u => u.Idcv.Equals(idCv.Id)).ToList();

            if (dbAward.Count == 0)
            {
                return NotFound();
            }

            return dbAward;
        }

        // GET: api/Awards
        [HttpGet]
        [Authorize(Roles = "ad")]
        public async Task<ActionResult<IEnumerable<Award>>> GetAwards()
        {
            return await _context.Awards.ToListAsync();
        }

        // GET: api/Awards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Award>> GetAward(string id)
        {
            var award = await _context.Awards.FindAsync(id);

            if (award == null)
            {
                return NotFound();
            }

            return award;
        }

        // PUT: api/Awards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "emp")]
        public async Task<IActionResult> PutAward(string id, model_Award award)
        {
            var exitIdAward = await _context.Awards.FindAsync(id);
            if (exitIdAward == null)
            {
                return BadRequest();
            }
            exitIdAward.Name = award.Name;
            exitIdAward.StartDate = award.StartDate;
            exitIdAward.EndDate =  award.EndDate;
            exitIdAward.Description = award.Description;

            _context.Update(exitIdAward);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return  CreatedAtAction("GetAward", new { id = exitIdAward.Id }, exitIdAward);
        }

        // POST: api/Awards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        public async Task<ActionResult<Award>> PostAward(model_Award award)
        {

            string Awardid = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault().Id;
            Award awardDB = new Award()
            {
                Id = Awardid ,
                Idcv = idCv,
                Name = award.Name,
                StartDate = award.StartDate,
                EndDate = award.EndDate,
                Description = award.Description,
            };

            _context.Awards.Add(awardDB);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AwardExists(awardDB.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetAward", new { id = awardDB.Id }, awardDB);
        }

        // DELETE: api/Awards/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "emp")]
        public async Task<IActionResult> DeleteAward(string id)
        {
            var award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return NotFound();
            }

            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AwardExists(string id)
        {
            return _context.Awards.Any(e => e.Id == id);
        }
    }
}
