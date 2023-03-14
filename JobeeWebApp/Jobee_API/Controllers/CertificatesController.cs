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
using System.Runtime.ConstrainedExecution;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly Project_JobeeContext _context;

        public CertificatesController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/5/Certificate
        [HttpGet("listCertificates")]
        public async Task<ActionResult<List<Certificate>>> GetCertificatesByCVId()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            var dbCer = _context.Certificates.Where(u => u.Idcv.Equals(idCv.Id)).ToList();

            if (dbCer.Count == 0)
            {
                return NotFound();
            }

            return dbCer;
        }

        // GET: api/Certificates
        //[HttpGet]
        //[Authorize(Roles = "ad")]
        //public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
        //{
        //    return await _context.Certificates.ToListAsync();
        //}

        //[HttpPut]
        //[Route("VerifyCertificate/{id}")]
        //[Authorize(Roles ="ad")]
        //public async Task<IActionResult> VerifyCertificate(string id)
        //{
        //    var existIdCer = await _context.Certificates.FindAsync(id);
        //    if (existIdCer == null)
        //    {
        //        return BadRequest();
        //    }

        //    existIdCer.IsVertify = true;
        //    _context.Update(existIdCer);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CertificateExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return CreatedAtAction("GetCertificate", new { id = existIdCer.Id }, existIdCer);
        //}

            // GET: api/Certificates/5
            [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetCertificate(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);

            if (certificate == null)
            {
                return NotFound();
            }

            return certificate;
        }

        // PUT: api/Certificates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "emp")]
        public async Task<IActionResult> PutCertificate(string id, model_Certificate certificate)
        {
            var existCer = await _context.Certificates.FindAsync(id);
            if(existCer == null)
            {
                return BadRequest();
            }
            existCer.Name= certificate.Name;
            existCer.StartDate= certificate.StartDate;
            existCer.EndDate= certificate.EndDate;
            existCer.Url = certificate.Url;

            _context.Update(existCer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCertificate", new { id = existCer.Id }, existCer);
        }


        // POST: api/Certificates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "emp")]
        public async Task<ActionResult<Certificate>> PostCertificate(model_Certificate certificate)
        {
            string CerId = Guid.NewGuid().ToString();
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string idCv = _context.TbCvs.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault().Id;
            Certificate cer = new Certificate()
            {
                Id = CerId,
                Idcv = idCv,
                Name = certificate.Name,
                StartDate = certificate.StartDate,
                EndDate = certificate.EndDate,
                Url = certificate.Url,
                IsVertify = false
            };
            _context.Certificates.Add(cer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CertificateExists(cer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCertificate", new { id = cer.Id }, cer);
        }

        // DELETE: api/Certificates/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "emp, ad")]
        public async Task<IActionResult> DeleteCertificate(string id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificateExists(string id)
        {
            return _context.Certificates.Any(e => e.Id == id);
        }
    }
}
