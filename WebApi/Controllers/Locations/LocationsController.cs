using AccountCLF.Application.Contract.Locations;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Locations
{
    [Route("api/location")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IGenericRepository<Location> _genericRepository;
        private readonly IMapper _mapper;
        public LocationsController(IGenericRepository<Location> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _genericRepository.GetAllAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _genericRepository.GetByIdAsync(id);
            if (location == null)
                return NotFound();

            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(CreateLocationDto locationDto)
        {
            try
            {
                var location = _mapper.Map<Location>(locationDto);
                var createdLocation = await _genericRepository.AddAsync(location);
                return createdLocation;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating location: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            var existingLocation = await _genericRepository.GetByIdAsync(id);
            if (existingLocation == null)
                return NotFound();

          var mappedData=  _mapper.Map(location, existingLocation); // Optionally, update properties of existingLocation with properties of location

            await _genericRepository.UpdateAsync(id, mappedData);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _genericRepository.GetByIdAsync(id);
            if (location == null)
                return NotFound("Location not found");

        await _genericRepository.RemoveAsync(location);
           

            return Ok("Location Delete Successfully");
        }
    }
}