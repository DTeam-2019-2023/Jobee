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
    public class TbCvController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public TbCvController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/TbCvs
        [HttpGet]
        [Authorize(Roles = "ad, guest")]
        public async Task<ActionResult<IEnumerable<TbCv>>> GetTbCvs()
        {
            return await _context.TbCvs.ToListAsync();
        }

        // GET: api/TbCvs/5
        // admin và guest có thể sử dụng
        [HttpGet]
        [Route("GetSingleAuto")]
        [Route("Update")]
        public async Task<ActionResult<TbCv>> GetTbCvsById()
        {
            string iduser = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            /* HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;*/
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            var existIdCv = await _context.TbCvs.FindAsync(idCv?.Id);
            var tbCv = await _context.TbCvs.FindAsync(existIdCv?.Id);

            if (tbCv == null)
            {
                return NotFound();
            }

            return tbCv;
        }

        // PUT: api/TbCvs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "emp")]
        public async Task<IActionResult> PutTbCv(CV cv)
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            var existIdCv = await _context.TbCvs.FindAsync(idCv.Id);
            if (existIdCv == null)
            {
                return BadRequest();
            }

            existIdCv.ApplyPosition = cv.ApplyPosition;
            existIdCv.CurrentJob = cv.CurrentJob;
            existIdCv.DesirySalary = cv.DesirySalary;
            existIdCv.Degree = cv.Degree;
            existIdCv.WorkExperience = cv.WorkExperience;
            existIdCv.DesiredWorkLocation = cv.DesiredWorkLocation;
            existIdCv.WorkingForm = cv.WorkingForm;
            existIdCv.CarrerObject = cv.CarrerObjiect;
            existIdCv.SoftSkill = cv.SoftSkill;
            existIdCv.Avatar = cv.Avatar;
            _context.Update(existIdCv);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TbCvExists(idCv.Id))
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

        //cấn chỗ idemployee

        // POST: api/TbCvs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        public async Task<ActionResult<TbCv>> PostTbCv(CV cv)
        {
            string Cvid = Guid.NewGuid().ToString();
            //lấy id user get từ jwt về
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var dbUser = _context.TbAccounts.Where(u => u.Username.Equals(iduser)).SingleOrDefault();
            if(dbUser != null)
            {
                return BadRequest("Cv exist");
            }
            TbCv cvDB = new TbCv()
            {
                Id = Cvid,
                Idaccount = iduser,
                ApplyPosition = cv.ApplyPosition,
                CurrentJob = cv.CurrentJob,
                DesirySalary = cv.DesirySalary,
                Degree = cv.Degree,
                WorkExperience = cv.WorkExperience,
                DesiredWorkLocation = cv.DesiredWorkLocation,
                WorkingForm = cv.WorkingForm,
                CarrerObject = cv.CarrerObjiect,
                SoftSkill = cv.SoftSkill,
                Avatar = cv.Avatar
            };
            _context.TbCvs.Add(cvDB);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TbCvExists(cvDB.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTbCv", new { id = cvDB.Id }, cvDB);
        }

        // DELETE: api/TbCvs/5
        //[HttpDelete("{id}")]
        //[Authorize(Roles = "emp")]
        //public async Task<IActionResult> DeleteTbCv(string id)
        //{
        //    var tbCv = await _context.TbCvs.FindAsync(id);
        //    if (tbCv == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TbCvs.Remove(tbCv);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool TbCvExists(string id)
        {
            return _context.TbCvs.Any(e => e.Id == id);
        }
    }
}
