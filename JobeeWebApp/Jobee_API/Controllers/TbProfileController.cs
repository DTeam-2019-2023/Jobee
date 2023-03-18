using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jobee_API.Entities;
using Jobee_API.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Profile = Jobee_API.Models.Profile;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TbProfileController : ControllerBase
    {
        private readonly Project_JobeeContext _context;


        public TbProfileController(Project_JobeeContext context)
        {
            _context = context;
        }

        // GET: api/TbProfiles
        //xem lại
        [HttpGet]
        [Authorize(Roles = "ad")]
        public async Task<ActionResult<IEnumerable<TbProfile>>> GetTbProfiles()
        {
            return await _context.TbProfiles.ToListAsync();
        }

        // GET: api/TbProfiles/5
        // admin và guest cũng có thể sử dụng
        [HttpGet]
        [Route("GetSingleAuto")]
        [Authorize]
        public async Task<ActionResult<TbProfile>> GetProfileById()
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            
            var idProfile = _context.TbProfiles.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            
            var existIdProfile = await _context.TbProfiles.FindAsync(idProfile?.Id);
            var tbPro = await _context.TbProfiles.FindAsync(existIdProfile?.Id);
            if (tbPro == null)
            {
                return NotFound();
            }
            return tbPro;
        }

        // PUT: api/TbProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "emp,ad")]
        [Route("Update")]
        public async Task<ActionResult<TbProfile>> PutTbProfile(Profile tbProfile)
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var existIdProfile = _context.TbProfiles.Where(u => u.Idaccount.Equals(iduser)).SingleOrDefault();
            //var existIdProfile = await _context.TbProfiles.FindAsync(idProfile.Id);
            if (existIdProfile == null)
            {
                string Profileid = Guid.NewGuid().ToString();
                TbProfile ProfileDB = new TbProfile()
                {
                    Id = Profileid,
                    Idaccount = iduser,
                    FirstName = tbProfile.FirstName,
                    LastName = tbProfile.LastName,
                    Gender = tbProfile.Gender,
                    DoB = tbProfile.DoB,
                    PhoneNumber = tbProfile.PhoneNumber,
                    Address = tbProfile.Address,
                    SocialNetwork = tbProfile.SocialNetwork,
                    DetailAddress = tbProfile.DetailAddress,
                    Email = tbProfile.Email
                };
                _context.TbProfiles.Add(ProfileDB);
                await _context.SaveChangesAsync();
                return ProfileDB;
            }

            existIdProfile.LastName = tbProfile.LastName;
            existIdProfile.FirstName = tbProfile.FirstName;
            existIdProfile.Address = tbProfile.Address;
            existIdProfile.PhoneNumber = tbProfile.PhoneNumber;
            existIdProfile.DoB = tbProfile.DoB;
            existIdProfile.Gender = tbProfile.Gender;
            existIdProfile.SocialNetwork = tbProfile.SocialNetwork;
            existIdProfile.DetailAddress = tbProfile.DetailAddress;
            existIdProfile.Email = tbProfile.Email;
            _context.Update(existIdProfile);

            await _context.SaveChangesAsync();
            

            return existIdProfile;
        }


        // POST: api/TbProfiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //[Authorize(Roles = "ad, emp")]
        //[Route("Create")]
        //public async Task<ActionResult<TbProfile>> PostTbProfile(Profile tbProfile)
        //{
        //    string Profileid = Guid.NewGuid().ToString();
        //    string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        //    var existProfileOfUser = _context.TbProfiles.Where(u => u.Idaccount.Equals(iduser)).FirstOrDefault();
        //    if (existProfileOfUser != null)
        //    {
        //        return Ok("da co profile roi");
        //    }
        //    TbProfile ProfileDB = new TbProfile()
        //    {
        //        Id = Profileid,
        //        Idaccount = iduser,
        //        FirstName = tbProfile.FirstName,
        //        LastName = tbProfile.LastName,
        //        Gender = tbProfile.Gender,
        //        DoB = tbProfile.DoB,
        //        PhoneNumber = tbProfile.PhoneNumber,
        //        Address = tbProfile.Address,
        //        SocialNetwork = tbProfile.SocialNetwork,
        //        DetailAddress = tbProfile.DetailAddress,
        //        Email = tbProfile.Email
        //    };
        //    _context.TbProfiles.Add(ProfileDB);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (TbProfileExists(ProfileDB.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return CreatedAtAction("GetTbProfile", new { id = ProfileDB.Id }, ProfileDB);
        //}


        // DELETE: api/TbProfiles/5
        //[HttpDelete("{id}")]
        //[Authorize(Roles = "ad")]
        //public async Task<IActionResult> DeleteTbProfile(string id)
        //{
        //    var tbProfile = await _context.TbProfiles.FindAsync(id);
        //    if (tbProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TbProfiles.Remove(tbProfile);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool TbProfileExists(string id)
        {
            return _context.TbProfiles.Any(e => e.Id == id);
        }
    }
}
