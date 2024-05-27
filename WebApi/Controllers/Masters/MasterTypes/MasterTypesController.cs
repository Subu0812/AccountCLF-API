using AccountCLF.Application.Contract.Masters.MasterType;
using AccountCLF.Data;
using AccountCLF.Data.Repository.MasterTypeDetails;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Masters.MasterTypes;

[Route("api/mastertype")]
[ApiController]
public class MasterTypesController : ControllerBase
{
    private readonly IGenericRepository<MasterType> _genericRepository;
    private readonly IMapper _mapper;
    private readonly IMasterTypeRepository _masterTypeRepository;
    public MasterTypesController(IGenericRepository<MasterType> genericRepository, IMapper mapper, IMasterTypeRepository masterTypeRepository)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
        _masterTypeRepository = masterTypeRepository;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetMasterTypeDto>>> GetMasterTypes()
    {
        var masterTypes = await _genericRepository.GetAllAsync();
        var mappedData = _mapper.Map<IEnumerable<GetMasterTypeDto>>(masterTypes);
        return Ok(mappedData);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetMasterTypeDto>> GetMasterType(int id)
    {
        var masterType = await _genericRepository.GetByIdAsync(id);
        if (masterType == null)
            return BadRequest("invalid Master Type Id!");

        var mappedData = _mapper.Map<GetMasterTypeDto>(masterType);
        return Ok(mappedData);
    }

    [HttpPost]
    public async Task<ActionResult<GetMasterTypeDto>> CreateMasterType(CreateMasterTypeDto masterType)
    {
        try
        {
            if (masterType.ParentId != null || masterType.ParentId != 0)
            {
                var selfType = await _genericRepository.GetByIdAsync((int)masterType.ParentId);
                if (selfType == null)
                {
                    return BadRequest($"invalid  Parent id {masterType.ParentId} ");
                }
            }
            var masterTypes = _mapper.Map<MasterType>(masterType);
            masterTypes.Date=DateTime.Now;
            var createdMasterType = await _genericRepository.AddAsync(masterTypes);
            var mappedData = _mapper.Map<GetMasterTypeDto>(createdMasterType);
            return mappedData;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating master type: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GetMasterTypeDto>> UpdateMasterType(int id, CreateMasterTypeDto masterTypeDto)
    {
        var existingMasterType = await _genericRepository.GetByIdAsync(id);
        if (existingMasterType == null)
            return BadRequest("Invalid Master Type Id");

        if (masterTypeDto.ParentId != null || masterTypeDto.ParentId == 0)
        {
            var selfType = await _genericRepository.GetByIdAsync((int)masterTypeDto.ParentId);
            if (selfType == null)
            {
                return BadRequest($"invalid  Parent id {masterTypeDto.ParentId} ");
            }
        }
        _mapper.Map(masterTypeDto, existingMasterType);

        var updatedMasterType = await _genericRepository.UpdateAsync(id, existingMasterType);

        if (updatedMasterType == null)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating master type");

        var updatedMasterTypeDto = _mapper.Map<GetMasterTypeDto>(updatedMasterType);
        return Ok(updatedMasterTypeDto);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterType(int id)
    {
        var masterType = await _genericRepository.GetByIdAsync(id);
        if (masterType == null)
            return BadRequest("invalid Master Type");

        await _genericRepository.RemoveAsync(masterType);
        return Ok("Master Type delete successfully");
    }

    [HttpPut]
    [Route("activate/deactivate")]
    public async Task<IActionResult> UpdateMasterTypeIsActive(int id)
    {
        var masterType = await _genericRepository.GetByIdAsync(id);
        if (masterType == null)
            return BadRequest("invalid Master Type");
        var data = await _masterTypeRepository.UpdateIsActive(id);
        return Ok($"Master type data is {data.IsActive}");
    }
}
