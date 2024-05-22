using AccountCLF.Application.Contract.Locations;
using AccountCLF.Application.Contract.Masters;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Locations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Locations;

[Route("api/location")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly IGenericRepository<Location> _genericRepository;
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MasterType> _masterGenericRepository;
    private readonly ILocationRepository _locationRepository;

    public LocationsController(IGenericRepository<Location> genericRepository, IMapper mapper, IGenericRepository<MasterType> masterGenericRepository,
        ILocationRepository locationRepository)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
        _masterGenericRepository = masterGenericRepository;
        _locationRepository = locationRepository;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetLocationDto>>> GetLocations()
    {
        var locations = await _locationRepository.Get();
        if (locations == null)
        {
            return NotFound("Data Not Found");
        }
        var mappedData = _mapper.Map<List<GetLocationDto>>(locations);
        return Ok(mappedData);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetLocationDto>> GetLocation(int id)
    {
        var location = await _locationRepository.GetById(id);
        if (location == null)
            return BadRequest("invalid Location Id");
        var mappedData = _mapper.Map<GetLocationDto>(location);

        return Ok(mappedData);
    }

    [HttpPut]
    [Route("activate/deactivate")]
    public async Task<IActionResult> UpdatIsActive(int id)
    {
        await _locationRepository.UpdateIsActive(id);
        return Ok("location Update Successfully");
    }

    [HttpPost]
    public async Task<ActionResult<GetLocationDto>> CreateLocation(CreateLocationDto locationDto)
    {
        try
        {
            var masterType = await _masterGenericRepository.GetByIdAsync((int)locationDto.TypeId);
            if (masterType == null)
            {
                return BadRequest("invalid Type id ");
            }
            if (locationDto.ParentId != null ||locationDto.ParentId!=0)
            {
                var location = await _genericRepository.GetByIdAsync((int)locationDto.ParentId);
                if (location == null)
                {
                    return BadRequest("invalid Type id ");
                }
            }
            var mappedData = _mapper.Map<Location>(locationDto);
            mappedData.IsActive = true;
            var createdLocation = await _genericRepository.AddAsync(mappedData);
            var GetMappedData = _mapper.Map<GetLocationDto>(createdLocation);

            return GetMappedData;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating location: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, CreateLocationDto location)
    {
        var existingLocation = await _genericRepository.GetByIdAsync(id);
        if (existingLocation == null)
            return BadRequest("invalid Location Id");

        var masterType = await _masterGenericRepository.GetByIdAsync((int)location.TypeId);
        if (masterType == null)
        {
            return BadRequest("invalid Type id ");
        }
        if (location.ParentId != null || location.ParentId != 0)
        {
            var locations = await _genericRepository.GetByIdAsync((int)location.ParentId);
            if (locations == null)
            {
                return BadRequest("invalid Type id ");
            }
        }
        var mappedData = _mapper.Map(location, existingLocation);
        await _genericRepository.UpdateAsync(id, mappedData);

        return Ok("Location update successfully");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _genericRepository.GetByIdAsync(id);
        if (location == null)
            return BadRequest(" invalid Location id");
        await _genericRepository.RemoveAsync(location);
        return Ok("Location Delete Successfully");
    }
}