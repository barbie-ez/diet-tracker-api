using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeightLossTracker.DataStore.Repositories.Interface;

namespace WeightLossTracker.Api.Controllers
{
    [Route("api/dietTracker")]
    [ApiController]
    public class DietTrackerController : ControllerBase
    {
        private IDietTrackerRepository _dietTrackerRepository;
        private readonly IMapper _mapper;
        public DietTrackerController(IDietTrackerRepository dietTrackerRepository, IMapper mapper)
        {
            _dietTrackerRepository = dietTrackerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DietTracker/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DietTracker
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/DietTracker/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
