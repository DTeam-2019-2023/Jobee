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
    public class ProjectController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public ProjectController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/ListProjects
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Project>> GetProjectsByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();

            if (idCv != null)
            {
                var dbProj = _context.Projects.Where(u => u.Idcv.Equals(idCv.Id)).ToList();
                if (dbProj.Count == 0)
                {
                    return NotFound();
                }
                return dbProj;
            }

            return default!;
        }

        // GET: api/Projects
        //[HttpGet]
        //[Authorize(Roles ="ad")]
        //public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        //{
        //    return await _context.Projects.ToListAsync();
        //}

        // GET: api/Projects/5
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<Project>> GetProject(string id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "emp")]
        [Route("UpdateById/{id}")]
        public async Task<IActionResult> PutProject(string id, model_Project project)
        {
            var exitIdProject = await _context.Projects.FindAsync(id);
            if (exitIdProject == null)
            {
                return BadRequest();
            }

            exitIdProject.Name = project.Name;
            exitIdProject.TeamSize = project.TeamSize;
            exitIdProject.Technology = project.Technology;
            exitIdProject .Role = project.Role;
            exitIdProject .StartDate = project.StartDate;
            exitIdProject.EndDate = project.EndDate;
            exitIdProject.Description = project.Description;

            _context.Update(exitIdProject);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        [Route("Create")]
        public async Task<ActionResult<Project>> PostProject(model_Project project)
        {
            string ProjectId = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var cv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            if (cv == null) return default!;

            Project DbProject = new Project()
            {
                Id = ProjectId,
                Idcv = cv.Id,
                Name = project.Name,    
                TeamSize = project.TeamSize,
                Role = project.Role,
                Technology = project.Technology,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Description = project.Description,

            };
           _context.Projects.Add(DbProject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectExists(DbProject.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProject", new { id = DbProject.Id }, DbProject);
        }

        // DELETE: api/Projects/5
        [HttpDelete]
        [Authorize(Roles = "emp")]
        [Route("Remove/{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(string id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
