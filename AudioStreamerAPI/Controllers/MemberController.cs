using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using AudioStreamerAPI.DTO;
using AutoMapper;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _repo;
        private readonly IMapper _mapper;

        public MemberController(IMemberRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            var members = _mapper.Map<IEnumerable<MemberDTO>>(_repo.GetMembers());
            return await Task.FromResult(members.ToList());
        }

        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> SearchMembers(string keyword)
        {
            var members = _mapper.Map<IEnumerable<MemberDTO>>(_repo.SearchMembers(keyword));
            return await Task.FromResult(members.ToList());
        }

        [HttpGet("id/{id}")]
        public IActionResult GetMember(int id)
        {
            MemberDTO? memberDTO = _mapper.Map<MemberDTO>(_repo.GetMember(id));
            var result = memberDTO != null ? new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = $"Found user with id: {id}.",
                Objects = new object[] { memberDTO },
            } : new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.NotFound,
                Message = $"User with id: {id} not found.",
            };
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("avatar/{id}")]
        public IActionResult GetMemberAvatarUrl(int id)
        {
            string url = _repo.GetMemberAvatarUrl(id);
            return Ok(new OperationalStatus
            {
                StatusCode = Constants.OperationalStatusEnums.Ok,
                Message = "Wanna know the name? Devil May Cry 3",
                Objects = new object[] { url },
            });
        }

        [HttpPatch("update")]
        public IActionResult UpdateMemberInfo([FromBody] MemberDTO memberDTO)
        {
            var member = _mapper.Map<Member>(memberDTO);
            var result = _repo.UpdateMember(member);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteMember(string email) {
            var result = _repo.DeleteMember(email);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
