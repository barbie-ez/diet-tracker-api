using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeightLossTracker.DataStore.DTOs;
using WeightLossTracker.DataStore.DTOs.Content;
using WeightLossTracker.DataStore.DTOs.Creation;
using WeightLossTracker.DataStore.Entitties;
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
        public async Task<IActionResult> GetDietEntrysForMember(string memberId)
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

            var dietEntry = _mapper.Map<IEnumerable<DietEntryDto>>(dietEntryFromRepo);

            return Ok(dietEntry);
        }

        [HttpGet("{id}", Name = "GetDietEntryForMember")]
        public async Task<IActionResult> GetDietEntryForMember(string memberId, int id)
        {
            var user = _memberManager.FindByIdAsync(memberId);
            if (user == null)
            {
                return NotFound();
            }

            var dietEntryFromRepo = await _dietTrackerRepository.GetFirstAsync(r => r.MemberId == memberId && r.Id==id);

            if (dietEntryFromRepo == null)
            {
                return NotFound();
            }

            var dietEntry = _mapper.Map<DietEntryDto>(dietEntryFromRepo);

            return Ok(dietEntry);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateDietEntryForMember(string memberId, [FromBody] DietEntryCreationDto dietEntry)
        {
            if(dietEntry == null)
            {
                return BadRequest();
            }

            var user = _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var dietEntryToAdd = _mapper.Map<DietTrackerModel>(dietEntry);

            var Id = await _dietTrackerRepository.AddReturnAsync(dietEntryToAdd);

            if(Id == 0)
            {
                throw new Exception("Creation of diet tracker failed due to ");
            }


            var dietEntryoReturn = _mapper.Map<DietEntryDto>(dietEntryToAdd);

            return CreatedAtRoute("GetDietEntryForMember",new { memberId =memberId, Id = Id}, dietEntryoReturn);
        }
    }
}
