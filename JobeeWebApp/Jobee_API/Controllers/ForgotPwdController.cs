using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jobee_API.Models;
using System.Text;
using Jobee_API.Entities;
using Jobee_API.Tools;
using MailKit;
using System.Security.Cryptography;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPwdController : ControllerBase
    {
        private readonly Project_JobeeContext _dbContext;
        private readonly IEmailService _emailService;
        public ForgotPwdController(Project_JobeeContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        class ReturnMailCheck
        {
            public string status { get; set; } = null!;
            public string message { get; set; } = null!;
            public string email { get; set; } = null!;
            public string? username { get; set; } = null!;

        }

        class ReturnLink
        {
            public string status { get; set; } = null!;
            public string message { get; set; } = null!;
            public string? link { get; set; }

        }
        string RandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }

        private async Task<TbAccount> getMailOwner(string mail)
        {
            try
            {
                var result = await _dbContext.TbProfiles.FirstAsync<TbProfile>(m => m.Email == mail);
                return await _dbContext.TbAccounts.FirstAsync<TbAccount>(m => m.Id == result.Idaccount);

                if (result == null)
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        // GET: api/ForgotPwd
        [HttpGet("checkMail/{email}")]
        public async Task<IActionResult> CheckMailExist(string email)
        {
            var result = await getMailOwner(email);
            if (result == null)
            {
                return NotFound(new ReturnMailCheck()
                {
                    status = "ERR",
                    email = email,
                    message = "This email does not exist in our database",
                    username = null
                });
            }

            //send mail
            string current_url = "localhost:7079/Account/CreateNewPassword?email=" + email + "&key=";
            var new_link = RandomString(255);
            var new_forgot_user = new TbForgotPwd()
            {
                Link = new_link,
                Uid = result.Id,
                ExpireDay = DateTime.UtcNow.AddDays(1.0),
            };
            _dbContext.TbForgotPwds.Add(new_forgot_user);
            await _dbContext.SaveChangesAsync();
            await _emailService.SendEmailAsync(
                new EmailData()
                {
                    EmailToId = email,
                    EmailSubject = "CV Profile Verify Email",
                    EmailBody = SendMailService.GetConfirmMail(result.Username, CALLBACK_URL: $"https://{current_url}{new_link}")
                }
                );

            return Ok(new ReturnMailCheck()
            {
                status = "OK",
                email = email,
                message = "Successfully find the owner of email",
                username = result.Id
            });
        }

        // GET: api/ForgotPwd/5
        //[HttpGet("getLink")]
        //public async Task<IActionResult> GetTbForgotPwd(string email, string uid)
        //{
        //    string current_url = "localhost:5151/api/ForgotPwd/recover?email=" + email + "&key=";
        //    var new_link = RandomString(255);
        //    var new_forgot_user = new TbForgotPwd()
        //    {
        //        Link = new_link,
        //        Uid = uid,
        //        ExpireDay = DateTime.UtcNow.AddDays(1.0),
        //    };
        //    try
        //    {
        //        _dbContext.TbForgotPwds.Add(new_forgot_user);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ReturnLink()
        //        {
        //            status = "ERR",
        //            link = null,
        //            message = "There is an un-expected error happened when we create your new link to recover password"
        //        });
        //    }

        //    return Ok(new ReturnLink()
        //    {
        //        status = "OK",
        //        message = "Successfully create new link for you to recover your password",
        //        link = current_url + new_link
        //    });
        //}


        // POST: api/ForgotPwd
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("isKeyExist")]
        public async Task<bool> isKeyExist(string key)
        {
            var forgot_user = await _dbContext.TbForgotPwds.FirstAsync<TbForgotPwd>(m => m.Link == key);
            return forgot_user != null;
        }

        [HttpPost("/recover")]
        public async Task<IActionResult> PostTbForgotPwd(string key, string new_pwd)
        {
            try
            {
                var forgot_user = await _dbContext.TbForgotPwds.FirstAsync<TbForgotPwd>(m => m.Link == key);
                if (forgot_user == null) return BadRequest(new { status = "ERR", message = "Can not find the key inside the database" });

                if (DateTime.UtcNow > forgot_user.ExpireDay)
                {
                    return BadRequest(new { status = "ERR", message = "The link is expired! Can not use this     link anymore!" });
                }

                var result = await _dbContext.TbAccounts.FirstAsync<TbAccount>(m => m.Id == forgot_user.Uid);
                if (result == null) return BadRequest(new { status = "ERR", message = "Fail to find the new user" });
                result.Passwork = new_pwd;
                _dbContext.Entry(result).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                _dbContext.TbForgotPwds.Remove(forgot_user);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }

            return Ok(new { status = "OK", message = "Successfully change your password" });
        }

        private bool TbForgotPwdExists(int id)
        {
            return _dbContext.TbForgotPwds.Any(e => e.Id == id);
        }
    }
}