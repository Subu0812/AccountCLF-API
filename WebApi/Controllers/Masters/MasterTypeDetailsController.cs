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
    public async Task<IEnumerable<GetMasterTypeDetailsDto>> GetMasterTypeDetails()
    {
        var MasterTypeDetails = await _masterTypeRepository.Get();
        var mappedData = _mapper.Map<IEnumerable<GetMasterTypeDetailsDto>>(MasterTypeDetails);
        return mappedData;
    }
    [HttpGet]
    [Route("mastertype/name")]
    public async Task<ActionResult<List<GetMasterTypeDetailsDto>>> GetMasterTypeDetailsByName(string name)
    {
        var MasterTypeDetails = await _masterTypeRepository.GetByTypeName(name);
        if (MasterTypeDetails == null)
        {
            return BadRequest($"Master Type Detail Not available with type name {name.ToUpper()}");
        }
        var mappedData = _mapper.Map<List<GetMasterTypeDetailsDto>>(MasterTypeDetails);
        return mappedData;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetMasterTypeDetailsDto>> GetMasterTypeByIdDetail(int id)
    {
        var MasterTypeDetail = await _masterTypeRepository.GetById(id);
        if (MasterTypeDetail == null)
            return BadRequest("invalid Master Type Detail Id! ");
        var mappedData = _mapper.Map<GetMasterTypeDetailsDto>(MasterTypeDetail);
        return Ok(mappedData);
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
            if(masterTypeDetail.ParentId.HasValue&&masterTypeDetail.ParentId!=0)
            {
                var selfType = await _genericRepository.GetByIdAsync((int)masterTypeDetail.ParentId);
                if (selfType == null)
                {
                    return BadRequest($"invalid  Parent id {masterTypeDetail.ParentId} ");
                }
            }
           
            var masterTypeDetails = _mapper.Map<MasterTypeDetail>(masterTypeDetail);
            masterTypeDetails.Date= DateTime.Now;
            var createdMasterTypeDetail = await _genericRepository.AddAsync(masterTypeDetails);
            return createdMasterTypeDetail;
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating master type Detail: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMasterTypeDetail(int id, CreateMasterTypeDetailDto MasterTypeDetail)
    {
        var data = await _genericRepository.GetByIdAsync(id);
        if (data == null)
            return BadRequest("invalid Master Type Detail");
        if (MasterTypeDetail.TypeId.HasValue)
        {
            var masterType = await _masterGenericRepository.GetByIdAsync((int)MasterTypeDetail.TypeId);
            if (masterType == null)
            {
                return BadRequest("invalid Type id ");
            }
        }
        if (MasterTypeDetail.ParentId.HasValue && MasterTypeDetail.ParentId != 0)
        {
            var selfType = await _genericRepository.GetByIdAsync((int)MasterTypeDetail.ParentId);
            if (selfType == null)
            {
                return BadRequest($"invalid  Parent id {MasterTypeDetail.ParentId} ");
            }
        }
        var masterTypeDetails = _mapper.Map(MasterTypeDetail, data);

        await _genericRepository.UpdateAsync(id, masterTypeDetails);

        return Ok("Master Type Updated!");
    }

    [HttpPut]
    [Route("activate/deactivate")]
    public async Task<IActionResult> UpdateMasterTypeDetailIsActive(int id)
    {
        var data = await _genericRepository.GetByIdAsync(id);
        if (data == null)
            return BadRequest("invalid Master Type Detail");
          await _masterTypeRepository.UpdateDetailsIsActive(id);
        return Ok($"Master type detail data is {data.IsActive}");
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