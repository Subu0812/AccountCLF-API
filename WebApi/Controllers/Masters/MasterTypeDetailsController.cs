using AccountCLF.Application.Contract.Masters;
using AccountCLF.Data;
using AccountCLF.Data.Repository.MasterTypeDetails;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Masters;

[Route("api/mastertypedetail")]
[ApiController]
public class MasterTypeDetailController : ControllerBase
{
    private readonly IGenericRepository<MasterTypeDetail> _genericRepository;
    private readonly IGenericRepository<MasterType> _masterGenericRepository;
    private readonly IMasterTypeRepository _masterTypeRepository;
    private readonly IMapper _mapper;
    public MasterTypeDetailController(IGenericRepository<MasterTypeDetail> genericRepository, IMapper mapper,
        IMasterTypeRepository masterTypeRepository, IGenericRepository<MasterType> masterGenericRepository)
    {
        _genericRepository = genericRepository;
        _mapper = mapper;
        _masterTypeRepository = masterTypeRepository;
        _masterGenericRepository = masterGenericRepository;
    }
    [HttpGet]
    public async Task<IEnumerable<MasterTypeDetail>> GetMasterTypeDetails()
    {
        var MasterTypeDetails = await _genericRepository.GetAllAsync();
        return MasterTypeDetails;
    }
    [HttpGet]
    [Route("mastertype/name")]
    public async Task<List<MasterTypeDetail>> GetMasterTypeDetailsByName(string name)
    {
        var MasterTypeDetails = await _masterTypeRepository.GetByTypeName(name);
        return MasterTypeDetails;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MasterTypeDetail>> GetMasterTypeByIdDetail(int id)
    {
        var MasterTypeDetail = await _genericRepository.GetByIdAsync(id);
        if (MasterTypeDetail == null)
            return BadRequest("invalid Master Type Detail Id! ");
        return Ok(MasterTypeDetail);
    }

    [HttpPost]
    public async Task<ActionResult<MasterTypeDetail>> CreateMasterTypeDetail(CreateMasterTypeDetailDto masterTypeDetail)
    {
        try
        {
            var masterType = await _masterGenericRepository.GetByIdAsync((int)masterTypeDetail.TypeId);
            if (masterType == null)
            {
                return BadRequest("invalid Type id ");
            }
            var masterTypeDetails = _mapper.Map<MasterTypeDetail>(masterTypeDetail);
            var createdMasterTypeDetail = await _genericRepository.AddAsync(masterTypeDetails);
            return createdMasterTypeDetail;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating master type Detail: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMasterTypeDetail(int id, MasterTypeDetail MasterTypeDetail)
    {
        var data = await _genericRepository.GetByIdAsync(id);
        if (data == null)
            return NotFound("Master Type Detail not found");

        await _genericRepository.UpdateAsync(id, MasterTypeDetail);

        return Ok("Master Type Updated!");
    }

    [HttpPut]
    [Route("activate/deactivate")]
    public async Task<IActionResult> UpdateMasterTypeDetailIsActive(int id, int isActive)
    {
        await _masterTypeRepository.UpdateIsActive(id, isActive);
        return Ok("data Update Successfully");
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMasterTypeDetail(int id)
    {
        var MasterTypeDetail = await _genericRepository.GetByIdAsync(id);
        if (MasterTypeDetail == null)
            return BadRequest("invalid Master Type Detail ");

        await _genericRepository.RemoveAsync(MasterTypeDetail);
        return Ok("Master Type Detail delete successfully");
    }
}