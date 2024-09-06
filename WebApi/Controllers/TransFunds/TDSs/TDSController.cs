using AccountCLF.Application.Contract.TransFunds.TDSs;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.TransFunds.TDSs
{
    [Route("api/tds")]
    [ApiController]
    public class TDSController : ControllerBase
    {
        private readonly IGenericRepository<TransFundTd> _transFundTdRepository;
        private readonly IGenericRepository<TransFund> _transFundGenericRepository;
        private readonly IMapper _mapper;

        public TDSController(IGenericRepository<TransFundTd> transFundTdRepository,
                             IGenericRepository<TransFund> transFundGenericRepository,
                             IMapper mapper)
        {
            _transFundTdRepository = transFundTdRepository;
            _transFundGenericRepository = transFundGenericRepository;
            _mapper = mapper;
        }

   

        [HttpPost]
        public async Task<ActionResult> CreateTDS(CreateTransFundTdsDto command)
        {
            var fundTrans = await _transFundGenericRepository.GetByIdAsync((int)command.FundReferenceId);
            if (fundTrans == null)
            {
                return BadRequest("Invalid FundReferenceId");
            }

            var transFundTd = _mapper.Map<CreateTransFundTdsDto, TransFundTd>(command);
            await _transFundTdRepository.AddAsync(transFundTd);
            return Ok("TDS Created Successfully!");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTDS(int id, CreateTransFundTdsDto command)
        {
            var existingTransFundTd = await _transFundTdRepository.GetByIdAsync(id);
            if (existingTransFundTd == null)
            {
                return NotFound("TDS record not found.");
            }

            var fundTrans = await _transFundGenericRepository.GetByIdAsync((int)command.FundReferenceId);
            if (fundTrans == null)
            {
                return BadRequest("Invalid FundReferenceId");
            }

            _mapper.Map(command, existingTransFundTd);
            await _transFundTdRepository.UpdateAsync(id, existingTransFundTd);
            return Ok("TDS Updated Successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTDS(int id)
        {
            var transFundTd = await _transFundTdRepository.GetByIdAsync(id);
            if (transFundTd == null)
            {
                return NotFound("TDS record not found.");
            }
            await _transFundTdRepository.RemoveAsync(transFundTd);
            return Ok("TDS Deleted Successfully!");
        }
    }
}
