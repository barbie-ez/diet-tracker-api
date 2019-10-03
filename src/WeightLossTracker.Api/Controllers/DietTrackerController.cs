using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeightLossTracker.DataStore.DTOs;
using WeightLossTracker.DataStore.Repositories.Impl;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/members/{memberId}/dietTracker")]
    [ApiController]
    public class DietTrackerController : ControllerBase
    {
        private IDietTrackerRepository _dietTrackerRepository;
        private MemberRepository _memberManager;
        private readonly IMapper _mapper;
        public DietTrackerController(IDietTrackerRepository dietTrackerRepository, MemberRepository memberManager, IMapper mapper)
        {
            _dietTrackerRepository = dietTrackerRepository;
            _memberManager = memberManager;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetDietEntryForMember(string memberId)
        {
            var user = _memberManager.FindByIdAsync(memberId);
            if (user == null)
            {
                return NotFound();
            }

            var dietEntryFromRepo = await _dietTrackerRepository.FindByAsync(r => r.MemberId == memberId);

            if (dietEntryFromRepo == null)
            {
                return NotFound();
            }

            var dietEntry = _mapper.Map<DietEntryDto>(dietEntryFromRepo);

            return Ok(dietEntry);
        }
    }
}
