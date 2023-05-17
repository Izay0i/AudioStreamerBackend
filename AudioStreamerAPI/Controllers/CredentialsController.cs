using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.Helpers;
using System.ComponentModel.DataAnnotations;
using AudioStreamerAPI.DTO;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialsController : ControllerBase
    {
        private readonly IMemberRepository _repo;

        public CredentialsController(IMemberRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public IActionResult Regiser([FromBody] CredentialsDTO credentials)
        {
            var member = _repo.GetMember(credentials.Email);
            if (member == null)
            {
                Member m = new() {
                    Email = credentials.Email,
                    Password = credentials.Password,
                    DisplayName = credentials.DisplayName,
                };

                if (m.DisplayName.Trim().Equals(string.Empty)) 
                {
                    return BadRequest(m.DisplayName);
                }
                else
                {
                    if (_repo.AddMember(m) == OperationalStatus.SUCCESS)
                    {
                        return Ok(credentials.Email);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return Conflict(credentials.Email);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CredentialsDTO credentials)
        {
            var member = _repo.GetMember(credentials.Email);
            if (member != null && 
                CredentialsHelper.VerifyPassword(member.Password, credentials.Password) == OperationalStatus.SUCCESS)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("change")]
        public IActionResult ChangePassword([FromBody] CredentialsDTO credentials, [MinLength(14)] string newPassword)
        {
            var member = _repo.GetMember(credentials.Email);
            if (member != null &&
                CredentialsHelper.VerifyPassword(member.Password, credentials.Password) == OperationalStatus.SUCCESS)
            {
                try
                {
                    var context = new fsnvdezgContext();
                    context.Members.Attach(member);
                    member.Password = CredentialsHelper.HashPassword(newPassword);
                    context.SaveChanges();
                    return Ok(member.Password);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Conflict(new object[] { credentials.Password, newPassword });
        }
    }
}
