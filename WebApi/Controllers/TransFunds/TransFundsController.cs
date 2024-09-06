using AccountCLF.Application.Contract.TransFunds;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers.TransFunds
{
    [Route("api/transfund")]
    [ApiController]
    public class TransFundsController : ControllerBase
    {
          private readonly IGenericRepository<TransFund> _transFundRepository;
        private readonly IGenericRepository<Entity> _entityRepository;
        private readonly IGenericRepository<AccountSession> _sessionRepository;
        private readonly IGenericRepository<MasterTypeDetail> _franchiseRepository;
        private readonly IGenericRepository<Entity> _staffRepository;
        private readonly IGenericRepository<Entity> _ledgerHeadRepository;
        private readonly IMapper _mapper;

        public TransFundsController(IGenericRepository<TransFund> transFundRepository, IGenericRepository<Entity> entityRepository, IGenericRepository<AccountSession> sessionRepository, IGenericRepository<MasterTypeDetail> franchiseRepository, IGenericRepository<Entity> staffRepository, IGenericRepository<Entity> ledgerHeadRepository, IMapper mapper)
        {
            _transFundRepository = transFundRepository;
            _entityRepository = entityRepository;
            _sessionRepository = sessionRepository;
            _franchiseRepository = franchiseRepository;
            _staffRepository = staffRepository;
            _ledgerHeadRepository = ledgerHeadRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTransFundDto>>> GetAllTransFunds()
        {
            var transFunds = await _transFundRepository.GetAllAsync();
            var transFundDtos = _mapper.Map<IEnumerable<TransFund>, IEnumerable<GetTransFundDto>>(transFunds);
            return Ok(transFundDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTransFundDto>> GetTransFundById(int id)
        {
            var transFund = await _transFundRepository.GetByIdAsync(id);
            if (transFund == null)
            {
                return NotFound("TransFund record not found.");
            }
            var transFundDto = _mapper.Map<TransFund, GetTransFundDto>(transFund);
            return Ok(transFundDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransFund(CreateTransFundDto command)
        {
            if (command.EntityId.HasValue)
            {
                var entity = await _entityRepository.GetByIdAsync(command.EntityId.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid EntityId");
                }
            }

            if (command.SessionId.HasValue)
            {
                var session = await _sessionRepository.GetByIdAsync(command.SessionId.Value);
                if (session == null)
                {
                    return BadRequest("Invalid SessionId");
                }
            }

            if (command.FranchiseId.HasValue)
            {
                var franchise = await _franchiseRepository.GetByIdAsync(command.FranchiseId.Value);
                if (franchise == null)
                {
                    return BadRequest("Invalid FranchiseId");
                }
            }

            if (command.StaffId.HasValue)
            {
                var staff = await _staffRepository.GetByIdAsync(command.StaffId.Value);
                if (staff == null)
                {
                    return BadRequest("Invalid StaffId");
                }
            }

            if (command.LedgerHeadId.HasValue)
            {
                var ledgerHead = await _ledgerHeadRepository.GetByIdAsync(command.LedgerHeadId.Value);
                if (ledgerHead == null)
                {
                    return BadRequest("Invalid LedgerHeadId");
                }
            }

            var transFund = _mapper.Map<CreateTransFundDto, TransFund>(command);
            await _transFundRepository.AddAsync(transFund);
            return Ok("TransFund Created Successfully!");
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransFund(int id, CreateTransFundDto command)
        {
            var existingTransFund = await _transFundRepository.GetByIdAsync(id);
            if (existingTransFund == null)
            {
                return NotFound("TransFund record not found.");
            }

            _mapper.Map(command, existingTransFund);
            await _transFundRepository.UpdateAsync(id,existingTransFund);
            return Ok("TransFund Updated Successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransFund(int id)
        {
            var transFund = await _transFundRepository.GetByIdAsync(id);
            if (transFund == null)
            {
                return NotFound("TransFund record not found.");
            }
            await _transFundRepository.RemoveAsync(transFund);
            return Ok("TransFund Deleted Successfully!");
        }
    }
}
