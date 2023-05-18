using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.Helpers;
using System.ComponentModel.DataAnnotations;
using AudioStreamerAPI.DTO;
using AudioStreamerAPI.Constants;

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
                    var result = _repo.AddMember(m);
                    return StatusCode((int)result.StatusCode, result.Message);
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
            if (member != null)
            {
                var result = CredentialsHelper.VerifyPassword(member.Password, credentials.Password);
                return StatusCode((int)result.StatusCode, result.Message);
            }
            return BadRequest(credentials.Email);
        }

        [HttpPost("change")]
        public IActionResult ChangePassword([FromBody] CredentialsDTO credentials, [MinLength(14)] string newPassword)
        {
            var member = _repo.GetMember(credentials.Email);
            if (member != null)
            {
                var result = CredentialsHelper.VerifyPassword(member.Password, credentials.Password);
                if (result.StatusCode == OperationalStatusEnums.Ok)
                {
                    try
                    {
                        var context = new fsnvdezgContext();
                        context.Members.Attach(member);
                        member.Password = CredentialsHelper.HashPassword(newPassword);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                return StatusCode((int)result.StatusCode, result.Message);
            }
            return Conflict(new object[] { credentials.Password, newPassword });
        }
    }
}
