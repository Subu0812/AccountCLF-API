using AccountCLF.Application.Contract.TransFunds.Loan;
using AccountCLF.Data;
using AccountCLF.Data.Repository.LoanAccounts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Security.Claims;


namespace WebApi.Controllers.TransFunds.Loans
{
    [Route("api/loan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanAccountRepository _loanRepository1;
        private readonly IGenericRepository<LoanAccountDetail> _loanAccountDetailRepository;
        private readonly IGenericRepository<Entity> _entityGenericRepository;
        private readonly IGenericRepository<LoanTenure> _loanTenureRepository;
        private readonly IGenericRepository<TransFund> _transFundGenericRepository;
        private readonly IGenericRepository<LoanAccount> _loanAccountRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoanController(ILoanAccountRepository loanRepository1,
            IGenericRepository<LoanAccountDetail> loanAccountDetailRepository,
            IGenericRepository<Entity> entityGenericRepository,
            IGenericRepository<LoanTenure> loanTenureRepository,
            IGenericRepository<TransFund> transFundGenericRepository,
            IGenericRepository<LoanAccount> loanAccountRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _loanRepository1 = loanRepository1;
            _loanAccountDetailRepository = loanAccountDetailRepository;
            _entityGenericRepository = entityGenericRepository;
            _loanTenureRepository = loanTenureRepository;
            _transFundGenericRepository = transFundGenericRepository;
            _loanAccountRepository = loanAccountRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }




        [HttpGet("user/loan/admin/account-report")]
        public async Task<ActionResult<List<GetTotalLoanNumberOfUserDto>>> GetLoanofUser()
        {
            var loanAccounts = await _loanRepository1.GetAsyncWithAll();

            if (loanAccounts == null || !loanAccounts.Any())
            {
                return NotFound("Data not found for Loan");
            }

            var reportList = new List<GetTotalLoanNumberOfUserDto>();

            foreach (var account in loanAccounts)
            {
                var entity = account.Entity;
                var basicProfile = entity?.BasicProfiles?.FirstOrDefault();
                var loanAccountDetail = account.LoanAccountDetails?.FirstOrDefault();
                var loanTenure = account.Loantenure;

                var report = new GetTotalLoanNumberOfUserDto
                {
                    Id = account.Id,
                    Name = basicProfile?.Name ?? "N/A",
                    LoanNo = account.SrNo,
                    LoanAmount = account.LoanAmount,
                    InterestAmount = (account.PayableAmount - account.LoanAmount) ?? 0,
                    PayableAmount = account.PayableAmount ?? 0,
                    StartDate = account.StartDate?.ToString("yyyy-MM-dd") ?? "N/A", 
                    EndDate = account.EndDate?.ToString("yyyy-MM-dd") ?? "N/A",     
                    InterestRate = loanAccountDetail?.InterestPercentage ?? 0,
                    LoanTenure = loanTenure?.Name ?? "N/A" ,
                    Status=(bool)account.Status
                };

                reportList.Add(report);
            }

            return Ok(reportList);
        }



        //[HttpGet("loanDetail/view/{loanaccountid}")]
        //public async Task<ActionResult<List<GetLoanDetailDto>>> GetLoanDetailByLoanAccountId(int loanaccountid)
        //{
        //    var loanAccountDetails = await _loanRepository1.GetLoanDetailByLoanAccountId(loanaccountid);
        //    if (loanAccountDetails == null || !loanAccountDetails.Any())
        //    {
        //        return NotFound($"Data not found for Loan Account Id ");
        //    }
        //    var reportList = new List<GetLoanDetailDto>();
        //    decimal PaidAmount = 0;
        //    decimal pendingAmount = 0;
        //    foreach (var account in loanAccountDetails)
        //    {
        //        if (account.Status == true)
        //        {
        //            PaidAmount += (decimal)account.PayableAmount;
        //        }
        //        else
        //        {
        //            pendingAmount += (decimal)account.PayableAmount;
        //        }

        //        var report = new GetLoanDetailDto
        //        {
        //            Id = account.Id,
        //            EmiMonth = account.EmiMonth,
        //            LoanAmount = account.LoanAmount,
        //            InterestAmount = account.InterestAmount,
        //            PayableAmount = account.PayableAmount,
        //            DueDate = account.DueDate,
        //            Status = account.Status,

        //        };
        //        reportList.Add(report);
        //    }
        //    return Ok(reportList);
        //}




        [HttpGet("loanDetail/loan/user/report/spare-extra")]
        public async Task<ActionResult<List<GetLoanDetailWithAllReportDto>>> GetLoanOfUser()
        {
            var loanAccounts = await _loanRepository1.GetAsyncWithAll();
            if (loanAccounts == null || !loanAccounts.Any())
            {
                return NotFound($"Data not found for entity ID ");
            }
            var entityIds = new HashSet<int>();
            var reportList = new List<GetLoanDetailWithAllReportDto>();
            foreach (var account in loanAccounts)
            {
                if (entityIds.Contains((int)account.EntityId))
                {
                    continue;
                }
                entityIds.Add((int)account.EntityId);

                decimal LoanAmount = 0;
                decimal PaidAmount = 0;
                decimal pendingAmount = 0;

                var LoanAccountDetails = await _loanRepository1.GetLoanDetailByEntityIdAsync((int)account.EntityId);
                foreach (var detail in LoanAccountDetails)
                {
                    if (detail.Status == true)
                    {
                        PaidAmount += (decimal)detail.PayableAmount;
                    }
                    else
                    {
                        pendingAmount += (decimal)detail.PayableAmount;
                    }
                }

                var report = new GetLoanDetailWithAllReportDto
                {
                    Name = account.Entity.BasicProfiles.FirstOrDefault()?.Name,
                    EntityId=(int)account.EntityId,
                    PendingAmount = pendingAmount,
                    PaidAmount = PaidAmount,
                    LoanAmount = PaidAmount + pendingAmount,
                    Status = account.Status,
                };
                reportList.Add(report);
            }
            return Ok(reportList);
        }



        [HttpGet("loanDetail/user/numberofloan")]
        public async Task<ActionResult<List<GetTotalLoanNumberOfUserDto>>> GetLoanNumberByEntityId(int? entityId)
        {

            int userId = 0;
            string userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("id");

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                userId = Convert.ToInt32(userIdClaim);
            }
            if (entityId.HasValue)
            {
                userId = entityId.Value;
            }
            if (userId == 0)
            {
                return BadRequest("Please provide any one valid entity ID or login to access the data.");
            }

            var loanAccounts = await _loanRepository1.GetByEntityIdAsync((int)entityId);

            if (loanAccounts == null || !loanAccounts.Any())
            {
                return NotFound($"Data not found for entity ID {entityId}");
            }

            var reportList = new List<GetTotalLoanNumberOfUserDto>();
            foreach (var account in loanAccounts)
            {
                var report = new GetTotalLoanNumberOfUserDto
                {
                    Id = account.Id,
                    LoanNo = account.SrNo,
                    LoanAmount = account.LoanAmount,
                    InterestAmount = account.PayableAmount - account.LoanAmount,
                    PayableAmount = account.PayableAmount,
                    StartDate = account.StartDate?.ToString("yyyy-MM-dd") ?? "N/A",
                    EndDate = account.EndDate?.ToString("yyyy-MM-dd") ?? "N/A",    
                    InterestRate = account.LoanAccountDetails.FirstOrDefault().InterestPercentage,
                    LoanTenure = account.Loantenure.Name
                };

                reportList.Add(report);
            }
            return Ok(reportList);
        }



        [HttpPost("createLoan")]
        public async Task<ActionResult> CreateLoanAccount([FromBody] LoanAccountDto loanAccountDto)
        {
            var entity = await _entityGenericRepository.GetByIdAsync(loanAccountDto.EntityId);
            if (entity == null)
            {
                return BadRequest("Invalid entity Id");
            }

            var loanTenure = await _loanTenureRepository.GetByIdAsync(loanAccountDto.LoantenureId);
            if (loanTenure == null)
            {
                return BadRequest("Invalid Loan Tenure Id");
            }
            var fundReference = await _transFundGenericRepository.GetByIdAsync(loanAccountDto.FundReferenceId);
            if (fundReference == null)
            {
                return BadRequest("Invalid Fund Reference Id");
            }

            var allLoanAccounts = await _loanAccountRepository.GetAllAsync();
            string newSrno = "0001";
            if (allLoanAccounts.Any())
            {
                var lastSrno = allLoanAccounts.OrderByDescending(l => l.SrNo).FirstOrDefault()?.SrNo;
                if (!string.IsNullOrEmpty(lastSrno))
                {
                    newSrno = (int.Parse(lastSrno) + 1).ToString("D4");
                }
            }

            var monthlyInterestRate = loanAccountDto.InterestPercentage / 12 / 100;


            decimal OnePlusRPowerN(decimal baseValue, int exponent)
            {
                decimal result = 1;
                for (int i = 0; i < exponent; i++)
                {
                    result *= baseValue;
                }
                return result;
            }

            var onePlusRPowerN = OnePlusRPowerN(1 + monthlyInterestRate, loanAccountDto.EmiMonth);

            var emi = (loanAccountDto.LoanAmount * monthlyInterestRate * onePlusRPowerN) / (onePlusRPowerN - 1);

            var outstandingPrincipal = loanAccountDto.LoanAmount;
            var totalPayableAmount = emi * loanAccountDto.EmiMonth;
            var totalInterestAmount = totalPayableAmount - loanAccountDto.LoanAmount;

            var loanAccount = new LoanAccount
            {
                SrNo = newSrno,
                LoanAmount = loanAccountDto.LoanAmount,
                PayableAmount = totalPayableAmount,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(loanAccountDto.EmiMonth),
                Status = true,
                EntityId = loanAccountDto.EntityId,
                FundReferenceId = loanAccountDto.FundReferenceId,
                LoantenureId = loanAccountDto.LoantenureId,
            };
            await _loanAccountRepository.AddAsync(loanAccount);

            for (int i = 0; i < loanAccountDto.EmiMonth; i++)
            {
                decimal interestPayment = outstandingPrincipal * monthlyInterestRate;
                decimal principalPayment = (decimal)emi - interestPayment;
                outstandingPrincipal -= principalPayment;

                var loanAccountDetail = new LoanAccountDetail
                {
                    LoanAccountId = loanAccount.Id,
                    EmiMonth = i + 1,
                    LoanAmount = principalPayment,
                    InterestAmount = interestPayment,
                    PayableAmount = emi,
                    InterestPercentage = loanAccountDto.InterestPercentage,
                    StartDate = DateTime.Now.AddMonths(i),
                    DueDate = DateTime.Now.AddMonths(i + 1),
                    Status = false,
                    EntityId = null,
                    FundReferenceId = null
                };
                await _loanAccountDetailRepository.AddAsync(loanAccountDetail);
            }

            return Ok("Loan account and details created successfully.");
        }



        [HttpGet("calculateLoan/emi")]
        public ActionResult<LoanCalculationDto> CalculateLoan([FromQuery] decimal amount, [FromQuery] int month,
                                    [FromQuery] decimal interestPercentage)
        {
            if (amount <= 0 || month <= 0 || interestPercentage < 0)
            {
                return BadRequest("Invalid input parameters.");
            }
            decimal monthlyInterestRate = interestPercentage / 12 / 100;
            decimal onePlusRPowerN = (decimal)Math.Pow((double)(1 + monthlyInterestRate), month);
            decimal emi = (amount * monthlyInterestRate * onePlusRPowerN) / (onePlusRPowerN - 1);
            decimal totalAmountInterest = (emi * month) - amount;

            LoanCalculationDto loanCalculationDto = new LoanCalculationDto
            {
                EmiMonth = month,
                Amount = amount,
                InterestAmount = totalAmountInterest,
                TotalInterestAmount = amount + totalAmountInterest,
                EMIAmountMonth = emi
            };
            return Ok(loanCalculationDto);
        }


    }
}

