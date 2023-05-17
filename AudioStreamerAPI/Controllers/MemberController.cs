﻿using AudioStreamerAPI.Models;
using AudioStreamerAPI.Repositories;
using AudioStreamerAPI.Constants;
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
            var members = _mapper.Map<IEnumerable<MemberDTO>>(_repo.GetMembers());
            return await Task.FromResult(members.ToList());
        }

        [HttpGet("id/{id}")]
        public IActionResult GetMember(int id)
        {
            MemberDTO? memberDTO = _mapper.Map<MemberDTO>(_repo.GetMember(id));
            if (memberDTO != null)
            {
                return Ok(memberDTO);
            }
            return NotFound(id);
        }

        [HttpPut("update")]
        public IActionResult UpdateMemberInfo([FromBody] MemberDTO memberDTO)
        {
            var member = _mapper.Map<Member>(memberDTO);
            if (_repo.UpdateMember(member) == OperationalStatus.SUCCESS)
            {
                return Ok(member);
            }
            return NotFound(new object[] { member.Email, member.Password, "Invalid credentials." });
        }

        [HttpDelete("delete")]
        public IActionResult DeleteMember(string email) {
            if (_repo.DeleteMember(email) == OperationalStatus.SUCCESS)
            {
                return Ok(email);
            }
            return NotFound(email);
        }
    }
}
