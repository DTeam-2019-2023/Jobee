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
    public class ActivitiesController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public ActivitiesController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/listActivities
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Activity>> GetActivitiesByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();

            if (idCv != null)
            {
                var dbAc = _context.Activities.Where(u => u.Idcv.Equals(idCv.Id)).ToList();
                if(dbAc.Count == 0)
                {
                    return NotFound();
                }
            }

            return default!;
        }

        // GET: api/Activities
        //[HttpGet]
        //[Authorize(Roles ="ad")]
        //public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
        //{
        //    return await _context.Activities.ToListAsync();
        //}

        // GET: api/Activities/5
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<Activity>> GetActivity(string id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        // PUT: api/Activities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "emp")]
        [Route("UpdateById/{id}")]

        public async Task<ActionResult<Activity>> PutActivity(string id, model_Activity activity)
        {
            var existIdAc = await _context.Activities.FindAsync(id);
            if (existIdAc == null)
            {
                return BadRequest();
            }

            existIdAc.Name = activity.Name;
            existIdAc.Role = activity.Role;
            existIdAc.StartDate = activity.StartDate;
            existIdAc.EndDate = activity.EndDate;
            existIdAc.Description = activity.Description;

            return existIdAc;
        }

        // POST: api/Activities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        [Route("Create")]
        public async Task<ActionResult<Activity>> PostActivity(model_Activity activity)
        {

            string DBId = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var cv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            if (cv == null) return default!;

            Activity DbAc = new Activity()
            {
                Id = DBId,
                Idcv = cv.Id,
                Name = activity.Name,
                Role = activity.Role,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                Description = activity.Description,
            };

            _context.Activities.Add(DbAc);
           
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ActivityExists(DbAc.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetActivity", new { id = DbAc.Id }, DbAc);
        }

        // DELETE: api/Activities/5
        [HttpDelete]
        [Authorize(Roles = "emp")]
        [Route("Remove/{id}")]

        public async Task<IActionResult> DeleteActivity(string id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(string id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
