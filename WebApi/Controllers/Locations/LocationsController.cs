using AccountCLF.Application.Contract.Locations;
using AccountCLF.Data;
using AccountCLF.Data.Repositories.Locations;
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
        private readonly IGenericRepository<MasterType> _masterGenericRepository;
        private readonly ILocationRepository _locationRepository;

        public LocationsController(IGenericRepository<Location> genericRepository, IMapper mapper, IGenericRepository<MasterType> masterGenericRepository, ILocationRepository locationRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _masterGenericRepository = masterGenericRepository;
            _locationRepository = locationRepository;
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

        [HttpPut]
        [Route("activate/deactivate")]
        public async Task<IActionResult> UpdatIsActive(int id, int isActive)
        {
            await _locationRepository.UpdateStatus(id, isActive);
            return Ok("location Update Successfully");
        }

        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(CreateLocationDto locationDto)
        {
            try
            {
                var masterType = await _masterGenericRepository.GetByIdAsync((int)locationDto.TypeId);
                if (masterType == null)
                {
                    return BadRequest("invalid Type id ");
                }
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

          var mappedData=  _mapper.Map(location, existingLocation); 
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