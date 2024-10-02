using AccountCLF.Application.Contract.Charges_Expenses;
using AccountCLF.Application.Contract.TransFunds.TDSs;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Daybooks;
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
        private readonly IDayBookRepository _dayBookRepository;

        public TDSController(IGenericRepository<TransFundTd> transFundTdRepository,
                             IGenericRepository<TransFund> transFundGenericRepository,
                             IMapper mapper,
                             IDayBookRepository dayBookRepository)
        {
            _transFundTdRepository = transFundTdRepository;
            _transFundGenericRepository = transFundGenericRepository;
            _mapper = mapper;
            _dayBookRepository = dayBookRepository;
        }




        [HttpGet]
        [Route("user-tds/report")]
        public async Task<ActionResult<IEnumerable<GetUserTDSDetailsDto>>> GetTDSReport(int entityId)
        {
            var daybooksList = await _dayBookRepository.GetAccountBalance();
            if (daybooksList == null || !daybooksList.Any())
            {
                return NotFound("No tds found for the specified EntityId.");
            }
            var filteredTdsList = daybooksList
           .Where(db => db.FranchiseId == entityId && db.Account?.Name.ToLower() == "tds charges")
           .Select(db => 
           {
                var transFundTds = db.FundReference?.TransFundTds?.FirstOrDefault(x => x.FundReferenceId == db.FundReferenceId);
               return new GetUserTDSDetailsDto
               {
                   Date = db.FundReference.Date ?? db.FundReference.EntryDate,
                   ParticularName = db.Franchise?.Name ?? "Unknown",
                   Section = transFundTds?.Section?.Name ?? "Unknown",
                   TaxAmount = transFundTds?.TdsableAmount ?? 0,
                   TdsPayable = transFundTds?.Tds ?? 0
               };
           })
           .ToList();
            if (!filteredTdsList.Any())
            {
                return NotFound("No TDS records found for the specified EntityId.");
            }
            return Ok(filteredTdsList);
        }




    }
}
