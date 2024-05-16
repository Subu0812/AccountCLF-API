using AccountCLF.Application.Contract.Masters;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Masters
{
    [Route("api/mastertypedetail")]
    [ApiController]
    public class MasterTypeDetailController : ControllerBase
    {
        private readonly IGenericRepository<MasterTypeDetail> _genericRepository;
        private readonly IMapper _mapper;
        public MasterTypeDetailController(IGenericRepository<MasterTypeDetail> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterTypeDetail>>> GetMasterTypeDetails()
        {
            var MasterTypeDetails = await _genericRepository.GetAllAsync();
            return Ok(MasterTypeDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasterTypeDetail>> GetMasterTypeDetail(int id)
        {
            var MasterTypeDetail = await _genericRepository.GetByIdAsync(id);
            if (MasterTypeDetail == null)
                return NotFound();

            return Ok(MasterTypeDetail);
        }

        [HttpPost]
        public async Task<ActionResult<MasterTypeDetail>> CreateMasterTypeDetail(CreateMasterTypeDetailDto masterTypeDetail )
        {
            try
            {
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterTypeDetail(int id)
        {
            var MasterTypeDetail = await _genericRepository.GetByIdAsync(id);
            if (MasterTypeDetail == null)
                return NotFound("Master Type Detail not found");

            await _genericRepository.RemoveAsync(MasterTypeDetail);
            return Ok("Master Type Detail delete successfully");
        }
    }
}