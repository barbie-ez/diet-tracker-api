using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WeightLossTrackeData.Repositories.Impl;
using WeightLossTrackerData.DTOs.Content;
using WeightLossTrackerData.DTOs.Creation;
using WeightLossTrackerData.Entities;
using WeightLossTrackerData.Repositories.Interface;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/members/{memberId}/weightHistory")]
    [ApiController]
    public class WeightHistoryController : Controller
    {
        private IWeightHistoryRepository _weightHistoryRepository;
        private MemberRepository _memberManager;
        private readonly IMapper _mapper;
        public WeightHistoryController(IWeightHistoryRepository weightHistoryRepository, MemberRepository member,
            IMapper mapper)
        {
            _weightHistoryRepository = weightHistoryRepository;
            _memberManager = member;
            _mapper = mapper;
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<WeightHistoriesDto>>> GetWeightHistoriesForMember(string memberId)
        {
            var user = _memberManager.FindByIdAsync(memberId);
            if (user == null)
            {
                return NotFound();
            }

            var weightFromRepo = await _weightHistoryRepository.FindByAsync(r => r.UserId == memberId);

            if (weightFromRepo == null)
            {
                return NotFound();
            }

            var weightEntry = _mapper.Map<IEnumerable<WeightHistoriesDto>>(weightFromRepo);

            return Ok(weightEntry);
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet("{id}",Name = "GetWeightHistoryForMember")]
        public async Task<ActionResult<WeightHistoriesDto>> GetWeightHistoryForMember(string memberId,int id)
        {
            var user = _memberManager.FindByIdAsync(memberId);
            if (user == null)
            {
                return NotFound();
            }

            var weightFromRepo = await _weightHistoryRepository.GetFirstAsync(r => r.UserId == memberId&& r.Id==id);

            if (weightFromRepo == null)
            {
                return NotFound();
            }

            var weightEntry = _mapper.Map<WeightHistoriesDto>(weightFromRepo);

            return Ok(weightEntry);
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/xml", "application/json")]
        [HttpPost()]
        public async Task<ActionResult<WeightHistoriesDto>> AddWeightHistoryForMember(string memberId, [FromBody] WeightHistoriesCreationDto weightHistoryDTO)
        {
            if (weightHistoryDTO == null)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var weightToAdd = _mapper.Map<WeightHistories>(weightHistoryDTO);

            user.WeightHistories.Add(weightToAdd);

            var Id = await _memberManager.UpdateAsync(user);

            if (!Id.Succeeded)
            {
                throw new Exception("Creation of diet tracker failed");
            }


            var weightToReturn = _mapper.Map<WeightHistoriesDto>(weightHistoryDTO);

            return CreatedAtRoute("GetWeightHistoryForMember", new { memberId = memberId, Id = Id }, weightToReturn);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPut("{id}", Name = "UpdateWeightHistoryForMember")]
        public async Task<ActionResult<WeightHistoriesDto>> UpdateWeightHistoryForMember(string memberId, int id, [FromBody] WeightHistoriesCreationDto weightHistoryDTO)
        {
            if (weightHistoryDTO == null)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var weightForMember = await _weightHistoryRepository.GetFirstAsync(r => r.UserId == user.Id&& r.Id==id);

            if (weightForMember == null)
            {
                return NotFound();
            }

            _mapper.Map(weightHistoryDTO, weightForMember);

            try
            {
                await _weightHistoryRepository.UpdateAsync(weightForMember);
            }
            catch (Exception)
            {
                throw new Exception("update weight failed");
            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPatch("{id}", Name = "PartiallyUpdateWeightHistoryForMember")]
        public async Task<ActionResult<WeightHistoriesDto>> PartiallyUpdateWeightHistoryForMember(string memberId, int id, [FromBody] JsonPatchDocument patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var weightForMember = await _weightHistoryRepository.GetFirstAsync(r => r.UserId == user.Id&& r.Id==id);

            if (weightForMember == null)
            {
                return NotFound();
            }

            var weightToPatch = _mapper.Map<WeightHistoriesCreationDto>(weightForMember);

            patchDoc.ApplyTo(weightToPatch);

            _mapper.Map(weightToPatch, weightForMember);

            try
            {
                await _weightHistoryRepository.UpdateAsync(weightForMember);
            }
            catch (Exception)
            {
                throw new Exception("update weight failed");
            }

            return NoContent();
        }
    }
}