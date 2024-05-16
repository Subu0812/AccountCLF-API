using AccountCLF.Application.Contract.Masters.MasterType;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Masters.MasterTypes
{
    [Route("api/mastertype")]
    [ApiController]
    public class MasterTypesController : ControllerBase
    {
        private readonly IGenericRepository<MasterType> _genericRepository;
        private readonly IMapper _mapper;
        public MasterTypesController(IGenericRepository<MasterType> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterType>>> GetMasterTypes()
        {
            var masterTypes = await _genericRepository.GetAllAsync();
            return Ok(masterTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MasterType>> GetMasterType(int id)
        {
            var masterType = await _genericRepository.GetByIdAsync(id);
            if (masterType == null)
                return NotFound();

            return Ok(masterType);
        }

        [HttpPost]
        public async Task<ActionResult<MasterType>> CreateMasterType(CreateMasterTypeDto masterType)
        {
            try
            {
                var masterTypes = _mapper.Map<MasterType>(masterType);
                var createdMasterType = await _genericRepository.AddAsync(masterTypes);
                return createdMasterType;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating master type: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMasterType(int id, MasterType masterType)
        {
            var data = await _genericRepository.GetByIdAsync(id);
            if (data == null)
                return NotFound("Master Type not found");

            await _genericRepository.UpdateAsync(id, masterType);

            return Ok("Master Type Updated!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterType(int id)
        {
            var masterType = await _genericRepository.GetByIdAsync(id);
            if (masterType == null)
                return NotFound("Master Type not found");

            await _genericRepository.RemoveAsync(masterType);
            return Ok("Master Type delete successfully");
        }
    }
}
