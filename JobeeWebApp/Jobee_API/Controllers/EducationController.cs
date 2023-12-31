﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jobee_API.Entities;
using Jobee_API.Models;
using System.Runtime.ConstrainedExecution;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public EducationController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/Educations
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Education>> GetEducationsByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            if (idCv != null)
            {
                var dbEdu = _context.Educations.Where(u => u.Idcv.Equals(idCv.Id)).ToList();
                if (dbEdu.Count == 0)
                {
                    return NotFound();
                }

                return dbEdu;
            }

            return default!;
        }

        // GET: api/Education
        //[HttpGet]
        //[Authorize(Roles ="ad")]
        //public async Task<ActionResult<IEnumerable<Education>>> GetEducations()
        //{
        //    return await _context.Educations.ToListAsync();
        //}

        // GET: api/Education/5
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<Education>> GetEducation(string id)
        {
            var education = await _context.Educations.FindAsync(id);

            if (education == null)
            {
                return NotFound();
            }

            return education;
        }

        // PUT: api/Education/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles ="emp")]
        [Route("UpdateById/{id}")]
        public async Task<ActionResult<Education>> PutEducation(string id, model_Education education)
        {
            var existEdu = await _context.Educations.FindAsync(id);
            if (existEdu == null)
            {
                return BadRequest();
            }
            existEdu.Name= education.Name;
            existEdu.Major= education.Major;
            existEdu.StartDate= education.StartDate;
            existEdu.EndDate= education.EndDate;
            existEdu.Gpa = education.GPA;
            existEdu.Description= education.Description;

            _context.Update(existEdu);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EducationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return existEdu;
        }

        // POST: api/Education
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        [Route("Create")]
        public async Task<ActionResult<Education>> PostEducation(model_Education education)
        {
            string eduId = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var cv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            if (cv == null) return default!;
            
            Education edu = new Education()
            {
                Id= eduId,
                Idcv= cv.Id,
                Name= education.Name,
                Major=education.Major,
                StartDate=education.StartDate,
                EndDate=education.EndDate,
                Gpa=education.GPA,
                Description=education.Description
            };
            _context.Educations.Add(edu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EducationExists(edu.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEducation", new { id = edu.Id }, edu);
        }

        // DELETE: api/Education/5
        [HttpDelete]
        [Authorize(Roles = "emp")]
        [Route("Remove/{id}")]
        public async Task<IActionResult> DeleteEducation(string id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EducationExists(string id)
        {
            return _context.Educations.Any(e => e.Id == id);
        }
    }
}
