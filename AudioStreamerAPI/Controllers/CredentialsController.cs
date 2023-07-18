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
        public IActionResult Register([FromBody] CredentialsDTO credentials)
        {
            var member = _repo.GetMember(credentials.Email.Trim());
            if (member == null)
            {
                if (credentials.DisplayName.Trim().Equals(string.Empty)) 
                {
                    return BadRequest(new OperationalStatus
                    {
                        StatusCode = OperationalStatusEnums.BadRequest,
                        Message = "Display name can't be empty.",
                    });
                }

                Member m = new()
                {
                    Email = credentials.Email.Trim(),
                    Password = credentials.Password.Trim(),
                    DisplayName = credentials.DisplayName,
                };
                var result = _repo.AddMember(m);
                return StatusCode((int)result.StatusCode, result);
            }
            return Conflict(new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.Conflict,
                Message = "User has already registered.",
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CredentialsDTO credentials)
        {
            var member = _repo.GetMember(credentials.Email.Trim());
            if (member != null)
            {
                var result = CredentialsHelper.VerifyPassword(member.Password.Trim(), credentials.Password.Trim());
                return StatusCode((int)result.StatusCode, result.StatusCode == OperationalStatusEnums.Ok ? new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = "Welcome back.",
                    Objects = new object[] { member.MemberId },
                } : new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = "User not found.",
                });
            }
            return BadRequest(new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.BadRequest,
                Message = $"Couldn't find account with email address of: {credentials.Email}",
            });
        }

        [HttpPost("change")]
        public IActionResult ChangePassword([FromBody] CredentialsDTO credentials, [MinLength(LengthConstants.MIN_PASSWORD_LENGTH)] string newPassword)
        {
            var member = _repo.GetMember(credentials.Email.Trim());
            if (member != null)
            {
                var result = CredentialsHelper.VerifyPassword(member.Password.Trim(), credentials.Password.Trim());
                if (result.StatusCode == OperationalStatusEnums.Ok)
                {
                    try
                    {
                        var context = new fsnvdezgContext();
                        context.Members.Attach(member);
                        member.Password = CredentialsHelper.HashPassword(newPassword.Trim());
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new OperationalStatus
                        {
                            StatusCode = OperationalStatusEnums.BadRequest,
                            Message = ex.Message,
                        });
                    }
                }
                return StatusCode((int)result.StatusCode, result);
            }
            return NotFound(new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.NotFound,
                Message = "User not found.",
            });
        }
    }
}
