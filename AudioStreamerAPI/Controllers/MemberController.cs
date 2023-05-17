using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _repo;

        public MemberController(IMemberRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await Task.FromResult(_repo.GetMembers().ToList());
        }

        [HttpGet("{keyword}")]
        public async Task<ActionResult<IEnumerable<Member>>> SearchMembers(string keyword)
        {
            return await Task.FromResult(_repo.SearchMembers(keyword).ToList());
        }

        [HttpGet("id/{id}")]
        public IActionResult GetMember(int id)
        {
            Member? member = _repo.GetMember(id);
            if (member != null)
            {
                return Ok(member);
            }
            return NotFound(id);
        }

        [HttpPut]
        public IActionResult UpdateMemberInfo([FromBody] Member member)
        {
            if (_repo.UpdateMember(member) == OperationalStatus.SUCCESS)
            {
                return Ok(member);
            }
            return NotFound(new object[] { member.Email, member.Password, "Invalid credentials." });
        }

        [HttpDelete]
        public IActionResult DeleteMember(string email) {
            if (_repo.DeleteMember(email) == OperationalStatus.SUCCESS)
            {
                return Ok(email);
            }
            return NotFound(email);
        }
    }
}
