using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeightLossTracker.DataStore.DTOs.Content;
using WeightLossTracker.DataStore.DTOs.Creation;
using WeightLossTracker.DataStore.Entitties;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/mealcategories")]
    [ApiController]
    public class MealCategoriesController : Controller
    {

        private IMealCategoryRepository _mealCategory { get; set; }
        private readonly IMapper _mapper;
        public MealCategoriesController(IMealCategoryRepository mealCategory, IMapper mapper)
        {
            _mealCategory = mealCategory;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<MealCategoriesDto>>> GetMealCategories()
        {
            var mealCategoriesFromRepo = await _mealCategory.GetAllAsync();

            var mealCategoriesDTO = _mapper.Map<IEnumerable<MealCategoriesDto>>(mealCategoriesFromRepo);

            return Ok(mealCategoriesDTO);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/xml", "application/json")]
        [HttpGet("{id}", Name = "GetMealCategory")]
        public async Task<ActionResult<MealCategoriesDto>> GetMealCategory(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var mealCategoriesFromRepo = await _mealCategory.GetFirstAsync(r=>r.Id==id);

            if (mealCategoriesFromRepo == null)
            {
                return NotFound();
            }

            var mealCategoriesDTO = _mapper.Map<MealCategoriesDto>(mealCategoriesFromRepo);

            return Ok(mealCategoriesDTO);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/xml", "application/json")]
        [HttpPost()]
        public async Task<ActionResult<MealCategoriesDto>> CreateMealCategory([FromBody] MealCategoriesCreationDto mealCategoryDTO)
        {
            if (mealCategoryDTO == null)
            {
                return BadRequest();
            }

            var mealCategoryToAdd = _mapper.Map<MealCategoriesModel>(mealCategoryDTO);

            var id = await _mealCategory.AddReturnAsync(mealCategoryToAdd);

            if (id == 0)
            {
                throw new Exception("An error occured while creating this meal category");
            }

            var mealCatgeoryToReturn = _mapper.Map<MealCategoriesDto>(mealCategoryDTO);

            return CreatedAtRoute("GetMealCategory", new { id=id} ,mealCategoryDTO);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpPut("{id}", Name = "UpdateMealCategory")]
        public async Task<ActionResult<MealCategoriesDto>> UpdateMealCategory(int id, [FromBody] MealCategoriesCreationDto mealCategoryDTO)
        {
            if (mealCategoryDTO == null)
            {
                return BadRequest();
            }

            var mealCategoryToFromRepo = await _mealCategory.GetFirstAsync(r => r.Id == id);

            if (mealCategoryToFromRepo == null)
            {
                return NotFound();
            }

            var mealCategoryToUpdate = _mapper.Map(mealCategoryDTO,mealCategoryToFromRepo);

            try
            {
                await _mealCategory.UpdateAsync(mealCategoryToUpdate);
            }
            catch (Exception)
            {
                throw new Exception("could not update meal category");
            }

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/xml", "application/json")]
        [HttpDelete("{id}", Name = "DeleteMealCategory")]
        public async Task<ActionResult<MealCategoriesDto>> DeleteMealCategory(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var mealCategoriesFromRepo = await _mealCategory.GetFirstAsync(r => r.Id == id);

            if (mealCategoriesFromRepo == null)
            {
                return NotFound();
            }

            try
            {
                await _mealCategory.DeleteAsync(mealCategoriesFromRepo);
            }
            catch (Exception)
            {
                throw new Exception("deletion of meal categories failed");
            }

            return NoContent();
        }
    }
}