using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
    public class DietTrackerController : Controller
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet()]
        public async Task<ActionResult<DietEntryDto>> GetDietEntrysForMember(string memberId)
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

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet("{id}", Name = "GetDietEntryForMember")]
        public async Task<ActionResult<DietEntryDto>> GetDietEntryForMember(string memberId, int id)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/xml", "application/json")]
        [HttpPost()]
        public async Task<IActionResult> CreateDietEntryForMember(string memberId, [FromBody] DietEntryCreationDto dietEntry)
        {
            if(dietEntry == null)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var dietEntryToAdd = _mapper.Map<DietTrackerModel>(dietEntry);

            var Id = await _dietTrackerRepository.AddReturnAsync(dietEntryToAdd);

            if(Id == 0)
            {
                throw new Exception("Creation of diet tracker failed");
            }


            var dietEntryoReturn = _mapper.Map<DietEntryDto>(dietEntryToAdd);

            return CreatedAtRoute("GetDietEntryForMember",new { memberId =memberId, Id = Id}, dietEntryoReturn);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPut("{id}", Name = "UpdateDietEntryForMember")]
        public async Task<ActionResult<DietEntryDto>> UpdateDietEntryForMember(string memberId, int id, [FromBody] DietEntryCreationDto dietEntry)
        {
            if (dietEntry == null)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var dietForMember = await _dietTrackerRepository.GetFirstAsync(r => r.MemberId == user.Id &&r.Id==id);

            if (dietForMember == null)
            {
                return NotFound();
            }
            
            _mapper.Map(dietEntry, dietForMember);

            try
            {
                await _dietTrackerRepository.UpdateAsync(dietForMember);
            }
            catch (Exception)
            {
                throw new Exception("update of diet tracker failed");
            }

            return NoContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId">User of the diet tracker</param>
        /// <param name="id">the id of the diet entry you want to update</param>
        /// <param name="patchDoc">the set of operations to apply to the diet entry</param>
        /// <returns>an ActionResult of Type Author</returns>
        /// <remarks>
        /// Sample request
        /// this request updates the portion size of the food
        /// PATCH /members/{memberId}/dietTracker/{id}\
        /// [\
        ///     {\
        ///         "op":"replace",\
        ///         "path":"/PortionSize"\
        ///         "value":1\
        ///     }\
        /// ]
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPatch("{id}", Name = "PartiallyUpdateDietEntryForMember")]
        public async Task<ActionResult<DietEntryDto>> PartiallyUpdateDietEntryForMember(string memberId, int id, [FromBody]JsonPatchDocument patchDoc)
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

            var dietForMember = await _dietTrackerRepository.GetFirstAsync(r => r.MemberId == user.Id && r.Id == id);

            if (dietForMember == null)
            {
                return NotFound();
            }

            var dietEntryToPatch = _mapper.Map<DietEntryDto>(dietForMember);

            patchDoc.ApplyTo(dietEntryToPatch);

            _mapper.Map(dietEntryToPatch, dietForMember);

            try
            {
                await _dietTrackerRepository.UpdateAsync(dietForMember);
            }
            catch (Exception)
            {
                throw new Exception("update of diet tracker failed");
            }

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpDelete("{id}", Name = "DeleteDietEntryForMember")]
        public async Task<ActionResult<DietEntryDto>> DeleteDietEntryForMember(string memberId, int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = await _memberManager.FindByIdAsync(memberId);

            if (user == null)
            {
                return NotFound();
            }

            var dietForMember = await _dietTrackerRepository.GetFirstAsync(r => r.Id == 1d && r.MemberId==memberId);

            if (dietForMember == null)
            {
                return NotFound();
            }

            try
            {
                await _dietTrackerRepository.DeleteAsync(dietForMember);
            }
            catch (Exception)
            {
                throw new Exception("delete of diet entry failed");
            }

            return NoContent();
        }
    }
}
