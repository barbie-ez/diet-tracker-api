using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeightLossTracker.Api.Helpers;
using WeightLossTracker.DataStore.DTOs.Content;
using WeightLossTracker.DataStore.DTOs.Creation;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Helpers;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : Controller
    {
        private IFoodRepository _food { get; set; }
        private IUrlHelper _urlHelper { get; set; }
        private readonly IMapper _mapper;
        private ILogger<FoodController> _logger;
        public FoodController(IFoodRepository food, ILogger<FoodController> logger,IMapper mapper, IUrlHelper urlHelper)
        {
            _food = food;
            _mapper = mapper;
            _logger = logger;
            _urlHelper = urlHelper;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet(Name ="GetFoods")]
        public async Task<ActionResult<IEnumerable<FoodDto>>> GetFoods([FromQuery]ResourceParameters resourceParameters)
        {

            var foodFromRepo = await _food.GetAllAsync(resourceParameters);

            var prevPageLink = foodFromRepo.HasPrevious ?
                CreateResourceUri(resourceParameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = foodFromRepo.HasNext ?
                CreateResourceUri(resourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetaData = new
            {
                totalCount = foodFromRepo.TotalCount,
                pageSize = foodFromRepo.PageSize,
                currentPage = foodFromRepo.CurrentPage,
                totalPages = foodFromRepo.TotalPages,
                prevPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaData));

            var foodDTO = _mapper.Map<IEnumerable<FoodDto>>(foodFromRepo);

            return Ok(foodDTO);
        }

        private string CreateResourceUri(ResourceParameters resourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetFoods", new
                    {
                        pageNumber = resourceParameters.PageNumber - 1,
                        pageSize = resourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetFoods", new
                    {
                        pageNumber = resourceParameters.PageNumber + 1,
                        pageSize = resourceParameters.PageSize
                    });
                default:
                    return _urlHelper.Link("GetFoods", new
                    {
                        pageNumber = resourceParameters.PageNumber,
                        pageSize = resourceParameters.PageSize
                    });
            }
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet("{id}", Name = "GetFood")]
        public async Task<ActionResult<FoodDto>> GetFood(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var foodFromRepo = await _food.GetFirstAsync(r => r.Id == id);

            if (foodFromRepo == null)
            {
                return NotFound();
            }

            var foodDTO = _mapper.Map<FoodDto>(foodFromRepo);

            return Ok(foodDTO);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/xml", "application/json")]
        [HttpPost()]
        public async Task<ActionResult<FoodDto>> CreateFood([FromBody] FoodCreationDto foodCreationDTO)
        {
            if (foodCreationDTO == null)
            {
                return BadRequest();
            }

           
            var foodToAdd = _mapper.Map<FoodModel>(foodCreationDTO);

            var id = await _food.AddReturnAsync(foodToAdd);

            if (id == 0)
            {
                throw new Exception("An error occured while creating this meal category");
            }

            var foodToReturn = _mapper.Map<FoodDto>(foodCreationDTO);

            return CreatedAtRoute("GetFood", new { id = id }, foodToReturn);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPut("{id}", Name = "UpdateFood")]
        public async Task<ActionResult<FoodDto>> UpdateFood(int id, [FromBody] FoodCreationDto foodToUpdateDTO)
        {
            if (foodToUpdateDTO == null)
            {
                return BadRequest();
            }

            var foodFromRepo = await _food.GetFirstAsync(r => r.Id == id);

            if (foodFromRepo == null)
            {
                return NotFound();
            }

             _mapper.Map(foodToUpdateDTO, foodFromRepo);

            try
            {
                await _food.UpdateAsync(foodFromRepo);
            }
            catch (Exception)
            {
                throw new Exception("could not update food");
            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPatch("{id}", Name = "PartiallyUpdateFood")]
        public async Task<ActionResult<FoodDto>> PartiallyUpdateFood(int id, [FromBody] JsonPatchDocument patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var foodFromRepo =await _food.GetFirstAsync(r => r.Id == id);

            if (foodFromRepo == null)
            {
                return NotFound();
            }

            var foodToPatch = _mapper.Map<FoodCreationDto>(foodFromRepo);

            patchDoc.ApplyTo(foodToPatch);

            _mapper.Map(foodToPatch, foodFromRepo);

            try
            {
                await _food.UpdateAsync(foodFromRepo);
            }
            catch (Exception)
            {
                throw new Exception("could not update food");
            }

            return NoContent();
        }
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpDelete("{id}", Name = "DeleteFood")]
        public async Task<ActionResult<FoodDto>> DeleteFood(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var foodFromRepo = await _food.GetFirstAsync(r => r.Id == id);

            if (foodFromRepo == null)
            {
                return NotFound();
            }

            try
            {
                await _food.DeleteAsync(foodFromRepo);
            }
            catch (Exception)
            {
                throw new Exception("Could not delete this food");
            }

            _logger.LogInformation(100, $"Food with {foodFromRepo.Id} was just deleted");

            return NoContent();
        }


    }
}