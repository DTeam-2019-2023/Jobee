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
    public class AwardController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public AwardController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/Educations
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Award>> GetAwardsByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();

            if (idCv != null)
            {
                var dbAw = _context.Awards.Where(u => u.Idcv.Equals(idCv.Id)).ToList();
                if (dbAw.Count == 0)
                {
                    return NotFound();
                }
                return dbAw;
            }

            return default!;
        }

        // GET: api/Awards
        [HttpGet]
        [Authorize(Roles = "ad")]
        public async Task<ActionResult<IEnumerable<Award>>> GetAwards()
        {
            return await _context.Awards.ToListAsync();
        }

        // GET: api/Awards/5
        [HttpGet]
        [Route("GetById/{id}")]
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
        [HttpPut]
        [Authorize(Roles = "emp")]
        [Route("UpdateById/{id}")]
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
            exitIdAward.Role = award.Role;

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
        [Route("Create")]
        public async Task<ActionResult<Award>> PostAward(model_Award award)
        {

            string Awardid = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var cv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            if (cv == null) return default!;

            Award awardDB = new Award()
            {
                Id = Awardid ,
                Idcv = cv.Id,
                Name = award.Name,
                StartDate = award.StartDate,
                EndDate = award.EndDate,
                Description = award.Description,
                Role = award.Role
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
        [HttpDelete]
        [Authorize(Roles = "emp")]
        [Route("Remove/{id}")]
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
