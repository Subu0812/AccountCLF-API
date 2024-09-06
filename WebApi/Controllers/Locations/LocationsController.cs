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
    private readonly IGenericRepository<MasterTypeDetail> _masterGenericRepository;
    private readonly ILocationRepository _locationRepository;

    public LocationsController(IGenericRepository<Location> genericRepository, IMapper mapper, IGenericRepository<MasterTypeDetail> masterGenericRepository,
        ILocationRepository locationRepository)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
        _masterGenericRepository = masterGenericRepository;
        _locationRepository = locationRepository;
    }
    [HttpGet]
    public async Task<ActionResult<List<GetLocationDto>>> GetLocations()
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
        var location = await _locationRepository.GetById(id);
        if (location == null)
            return BadRequest("invalid Location Id");
      var data =  await _locationRepository.UpdateIsActive(id);
        return Ok($"location Data is {data.IsActive}");
    }

    [HttpPost]
    public async Task<ActionResult> CreateLocation(CreateLocationDto locationDto)
    {
        
            if (locationDto.TypeId.HasValue)
            {
                var masterType = await _masterGenericRepository.GetByIdAsync((int)locationDto.TypeId);
                if (masterType == null)
                {
                    return BadRequest("invalid Type id ");
                }
            }
            if (locationDto.ParentId.HasValue && locationDto.ParentId.Value != 0)
            {
                var location = await _genericRepository.GetByIdAsync((int)locationDto.ParentId);
                if (location == null)
                {
                    return BadRequest("invalid parent id ");
                }
            }
            var mappedData = _mapper.Map<Location>(locationDto);
            mappedData.IsActive = true;
            var createdLocation = await _genericRepository.AddAsync(mappedData);
            return Ok("Location Created successfully"); 
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
        if (location.ParentId.HasValue && location.ParentId.Value != 0)
        {
            var locations = await _genericRepository.GetByIdAsync((int)location.ParentId);
            if (locations == null)
            {
                return BadRequest("invalid parent id ");
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

    [HttpGet]
    [Route("state")]
    public async Task<ActionResult<List<GetLocationDto>>> Getstates()
    {
        var locations = await _locationRepository.GetState();
        if (locations == null)
        {
            return NotFound("Data Not Found");
        }
        var mappedData = _mapper.Map<List<GetLocationDto>>(locations);
        return Ok(mappedData);
    }

    [HttpGet]
    [Route("city/{stateId}")]
    public async Task<ActionResult<List<GetLocationDto>>> GetCityByStateId(int stateId)
    {
        var locations = await _locationRepository.GetCityByStateId(stateId);
        if (locations == null)
        {
            return NotFound("Data Not Found");
        }
        var mappedData = _mapper.Map<List<GetLocationDto>>(locations);
        return Ok(mappedData);
    }

    [HttpGet]
    [Route("block/{cityId}")]
    public async Task<ActionResult<List<GetLocationDto>>> GetBlockByCityId(int cityId)
    {
        var locations = await _locationRepository.GetBlockByCityId(cityId);
        if (locations == null)
        {
            return NotFound("Data Not Found");
        }
        var mappedData = _mapper.Map<List<GetLocationDto>>(locations);
        return Ok(mappedData);
    }
}