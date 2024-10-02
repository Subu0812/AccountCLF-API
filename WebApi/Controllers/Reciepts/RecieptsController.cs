using AccountCLF.Application.Contract;
using AccountCLF.Application.Contract.Charges_Expenses;
using AccountCLF.Application.Contract.Reciepts;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Daybooks;
using AccountCLF.Data.Repository.Entities;
using AccountCLF.Data.Repository.LoanAccounts;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Model;
using System.Security.Claims;

namespace WebApi.Controllers.Reciepts
{
    [Route("api/reciept")]
    [ApiController]
    public class RecieptsController : ControllerBase
    {
        private readonly AccountClfContext _dbContext;
        private readonly IGenericRepository<TransFund> _transFundRepository;
        private readonly IGenericRepository<Daybook> _dayBookGenericRepository;
        private readonly IGenericRepository<Entity> _entityGenericRepository;
        private readonly IGenericRepository<MasterTypeDetail> _masterTypeDetailRepository;
        private readonly IGenericRepository<AccountSession> _accountSessionRepository;
        private readonly IGenericRepository<VoucherSrNo> _voucherSrNoRepository;
        private readonly IGenericRepository<VoucherType> _voucherTypeRepository;
        private readonly IGenericRepository<LoanAccount> _loanAccountRepository;
        private readonly IGenericRepository<LoanAccountDetail> _loanAccountDetailRepository;
        private readonly IGenericRepository<TransFundTd> _tdsGenericRepository;
        private readonly IEntityRepository _entityRepository;
        private readonly ILoanAccountRepository _loanRepository1;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IDayBookRepository _dayBookRepository;

        public RecieptsController(
            IGenericRepository<TransFund> transFundRepository,
            IGenericRepository<Daybook> daybookGenericRepository,
            IGenericRepository<Entity> entityGEnericRepository,
            IGenericRepository<MasterTypeDetail> masterTypeDetailRepository,
            IGenericRepository<AccountSession> accountSessionRepository,
            IHttpContextAccessor contextAccessor,
            IGenericRepository<VoucherSrNo> voucherSrNoRepository,
            IGenericRepository<VoucherType> voucherTypeRepository,
            IGenericRepository<LoanAccount> loanAccountRepository,
            IGenericRepository<LoanAccountDetail> loanAccountDetailRepository,
            ILoanAccountRepository loanRepository1,
            IGenericRepository<TransFundTd> tdsGenericRepository,
            IEntityRepository entityRepository,
            IDayBookRepository dayBookRepository,
            AccountClfContext dbContext)
        {
            _transFundRepository = transFundRepository;
            _dayBookGenericRepository = daybookGenericRepository;
            _entityGenericRepository = entityGEnericRepository;
            _masterTypeDetailRepository = masterTypeDetailRepository;
            _accountSessionRepository = accountSessionRepository;
            _contextAccessor = contextAccessor;
            _voucherSrNoRepository = voucherSrNoRepository;
            _voucherTypeRepository = voucherTypeRepository;
            _loanAccountRepository = loanAccountRepository;
            _loanAccountDetailRepository = loanAccountDetailRepository;
            _loanRepository1 = loanRepository1;
            _tdsGenericRepository = tdsGenericRepository;
            _entityRepository = entityRepository;
            _dayBookRepository = dayBookRepository;
            _dbContext = dbContext;
        }






        [HttpGet("cash-bank/total-balance/paymode/{entityId}")]
        public async Task<ActionResult<GetAllUserAccountBalanceDto>> GetCashBankUserTotalBalance(int entityId, int? paymodeid,int bankid)
        {
            var entity = await _entityGenericRepository.GetByIdAsync(entityId);
            if (entity == null)
            {
                return BadRequest("Entity ID is not valid.");
            }   

            var filteredDaybooks = new List<Daybook>();
            var daybooks = await _dayBookRepository.GetAll();
            filteredDaybooks = daybooks
               .Where(d => d.FranchiseId == entityId)
               .ToList();
            var payModeData = new MasterTypeDetail();
            int accountid = 0;
            if (paymodeid != null)
            {
                payModeData = await _masterTypeDetailRepository.GetByIdAsync((int)paymodeid);
                filteredDaybooks = filteredDaybooks.Where(x => x.FundReference.PayMode == paymodeid).ToList();
            }
            if (bankid != null)
            {
                accountid = payModeData.Name.ToLower() == "cash" ? 236 : payModeData.Name.ToLower() == "bank" ? 237 : 0;
                filteredDaybooks = filteredDaybooks.Where(x=>x.AccountId== accountid).ToList(); 
            }
           
            if (!filteredDaybooks.Any())
            {
                return NotFound("No records found for the given Franchise ID and PayMode ID.");
            }

            decimal balance = 0;
            foreach (var daybook in filteredDaybooks)
            {
                if (daybook.TransType == "CR")
                {
                    balance += daybook.Amount ?? 0;
                }
                else if (daybook.TransType == "DR")
                {
                    balance -= daybook.Amount ?? 0;
                }
            }
            var totalBalance = new GetAllUserAccountBalanceDto
            {
                AccountBalance = balance >= 0
                               ? $"{balance:F2} CR"
                               : $"{-balance:F2} DR"
            };
            return Ok(totalBalance);
        }


        [HttpGet("balance/{entityId}")]
        public async Task<ActionResult<IEnumerable<BalanceDto>>> GetDaybookBalance(int entityId)

        {
            var entity = await _entityGenericRepository.GetByIdAsync(entityId);
            if (entity == null)
            {
                return BadRequest("entity id not valid");
            }
            var daybooks = await _dayBookRepository.GetAll();
            var filteredDaybooks = daybooks.Where(d => d.FranchiseId == entityId).ToList();

            if (!filteredDaybooks.Any())
            {
                return NotFound("No records found for the given Franchise ID.");
            }

            List<BalanceDto> balanceDtos = new List<BalanceDto>();
            decimal balance = 0;

            foreach (var daybook in filteredDaybooks)
            {
                if (daybook.TransType == "CR")
                {
                    balance -= daybook.Amount ?? 0;
                }
                else if (daybook.TransType == "DR")
                {
                    balance += daybook.Amount ?? 0;
                }
                balanceDtos.Add(new BalanceDto
                {
                    Id = daybook.Id,
                    FranchiseId = daybook.FranchiseId ?? 0,
                    TransType = daybook.TransType.ToString() ?? "*",
                    Amount = (daybook.Amount?.ToString("F2") + " " + daybook.TransType) ?? "0",
                    Balance = balance >= 0
            ? balance.ToString("F2") + " CR"
            : (-balance).ToString("F2") + " DR",
                    Date = daybook.FundReference.Date
                ?? daybook.FundReference.EntryDate
                ?? daybook.FundReference.PaymentConfirmDate
                });
            }
            return Ok(balanceDtos);
        }


        [HttpGet]
        [Route("cash-bank/account-balance/report/{entityid}")]
        public async Task<ActionResult<IEnumerable<GetAccountBalanceReportDto>>> GetCashBankAccountBalanceReport(int entityid, int? paymodeid)
        {
            try
            {
                var daybooks = await _dayBookRepository.GetAccountBalance();
                if (daybooks == null || !daybooks.Any())
                {
                    return BadRequest("No transactions found for the specified EntityId.");
                }
                var filteredDaybooks = new List<Daybook>();
                filteredDaybooks = daybooks.Where(x => x.FranchiseId == entityid).ToList();
                if (paymodeid != null)
                {
                    filteredDaybooks = filteredDaybooks.Where(x => x.FundReference.PayMode == paymodeid).ToList();
                }
                if (!filteredDaybooks.Any())
                {
                    return BadRequest("No  transactions found.");
                }
                var accountBalanceReportList = new List<GetAccountBalanceReportDto>();
                decimal currentBalance = 0;
                foreach (var daybook in filteredDaybooks)
                {
                    string profileName = "Unknown";
                    profileName = daybook.Franchise?.BasicProfiles.Where(x => x.EntityId == entityid)
                       .FirstOrDefault().Name ?? "Unknown";

                    var amount = daybook.Amount ?? 0;

                    if (daybook.TransType == "CR")
                    {
                        currentBalance += amount;
                    }
                    else if (daybook.TransType == "DR")
                    {
                        currentBalance -= amount;
                    }
                    accountBalanceReportList.Add(new GetAccountBalanceReportDto
                    {
                        Id = daybook.Id,
                        HolderName = profileName,
                        Amount = amount,
                        TransType = daybook.TransType,
                        AccountBalance = currentBalance >= 0
                        ? $"{currentBalance:F2} CR"
                        : $"{-currentBalance:F2} DR",
                        PayModeId = daybook.FundReference?.PayMode,
                        Date = daybook.FundReference?.Date ?? daybook.FundReference?.EntryDate
                    });
                }
                return Ok(accountBalanceReportList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<int>> CreateRecieptDetail(CreateRecieptDto command)
        {

            var loginIdClaim = _contextAccessor.HttpContext.User.FindFirstValue("id");
            int? loginId = null;
            if (!string.IsNullOrEmpty(loginIdClaim))
            {
                loginId = Convert.ToInt32(loginIdClaim);
            }
            if (loginId == null && loginId == 0)
            {
                return BadRequest("Please login first");
            }
            var checkAccountBalance = await _dayBookRepository.GetAccountBalanceByEntityIdAsync(command.EntityId.Value);
            if (checkAccountBalance < command.Amount && command.PayMode == 46)
            {
                return BadRequest("your amount is low in your account!");
            }
            var listMasterType = await _masterTypeDetailRepository.GetAllAsync();
            if (command.EntityId.HasValue)
            {
                var entity = await _entityGenericRepository.GetByIdAsync(command.EntityId.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid Entity Id");
                }
            }
            var payMode = new MasterTypeDetail();
            if (command.PayMode.HasValue)
            {
                payMode = await _masterTypeDetailRepository.GetByIdAsync((int)command.PayMode);
                if (payMode == null)
                {
                    return BadRequest("Invalid PayMode Id");
                }
            }
            var accountId = payMode.Name.ToLower() == "cash" ? 239 : payMode.Name.ToLower() == "bank" ? 237 : (int?)null;
            if (command.AccountId.HasValue)
            {
                var account = await _entityGenericRepository.GetByIdAsync((int)command.AccountId);
                if (account == null)
                {
                    return BadRequest("Invalid account Id");
                }
            }
            if (command.SessionId.HasValue)
            {
                var session = await _accountSessionRepository.GetByIdAsync(command.SessionId.Value);
                if (session == null)
                {
                    return BadRequest("Invalid Session Id");
                }
            }
            var ledgerHead = listMasterType.FirstOrDefault(m => m.Name.ToLower().Equals("receipt", StringComparison.OrdinalIgnoreCase));
            if (ledgerHead == null)
            {
                return BadRequest("ledgerHead with name 'receipt' not found");
            }
            var voucherSRNOs = await _voucherSrNoRepository.GetAllAsync();
            var srNo = voucherSRNOs.Select(pd => pd.SrNo).DefaultIfEmpty(0).Max();
            var newsrNo = srNo + 1;

            var orderNo = newsrNo.ToString().PadLeft(7, '0');
            var voucherTypeGet = await _voucherTypeRepository.GetAllAsync();
            var voucherTypeSelect = voucherTypeGet.FirstOrDefault(m => m.Name.ToLower().Equals("receipt"));
            if (voucherTypeSelect == null)
            {
                return BadRequest("Voucher Type with name 'receipt' not found");
            }
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var voucherNo = new VoucherSrNo
                    {
                        SessionId = command.SessionId,
                        ClfId = command.EntityId,
                        SrNo = newsrNo,
                        VoucherTypeId = voucherTypeSelect.Id,
                    };
                    var createdVoucherNo = await _voucherSrNoRepository.AddAsync(voucherNo);
                    var transFund = new TransFund
                    {
                        EntityId = loginId,
                        VoucherNo = createdVoucherNo.Id,
                        VoucherType = voucherTypeSelect.Id,
                        TotalAmount = command.Amount,
                        EntryDate = command.EntryDate,
                        Date = DateTime.Now,
                        SessionId = command.SessionId,
                        FranchiseId = command.EntityId,
                        StaffId = loginId,
                        PayMode = command.PayMode,
                        LedgerHeadId = ledgerHead.Id,
                        Status = true,
                        PaymentConfirmDate = DateTime.Now,
                        TaxableAmount = command.Amount,
                    };
                    var createdTransFund = await _transFundRepository.AddAsync(transFund);

                    var daybookCR = new Daybook
                    {
                        FundReferenceId = createdTransFund.Id,
                        AccountId = accountId,
                        Amount = command.RecieveAmount,
                        TransType = "CR",
                        FranchiseId = loginId,
                        StaffId = loginId,
                        SessionId = command.SessionId,
                        Status = true,
                        DemoId = command.DemoId
                    };
                    var createdDayBookCR = await _dayBookGenericRepository.AddAsync(daybookCR);

                    var daybookDR = new Daybook
                    {
                        FundReferenceId = createdTransFund.Id,
                        AccountId = accountId,
                        Amount = command.Amount,
                        TransType = "DR",
                        FranchiseId = command.EntityId,
                        StaffId = loginId,
                        SessionId = command.SessionId,
                        Status = true,
                        DemoId = command.DemoId,
                        ParentId = createdDayBookCR.Id,
                    };
                    var createdDayBookDR = await _dayBookGenericRepository.AddAsync(daybookDR);
                    if (command.TDSAmount.HasValue)
                    {
                        var entityTDSCharges = await _entityRepository.GetAll();
                        var tdschargeselect = entityTDSCharges
                               .Where(e => !string.IsNullOrEmpty(e.Name))
                               .FirstOrDefault(m => m.Name.ToLower().Equals("tds charges"));
                        var deduction = new Daybook
                        {
                            AccountId = tdschargeselect.Id,
                            Status = true,
                            TransType = "CR",
                            SessionId = command.SessionId,
                            Amount = command.TDSAmount,
                            FranchiseId = loginId,
                            FundReferenceId = createdTransFund.Id,
                            ParentId = createdDayBookCR.Id,
                            StaffId = loginId,
                        };
                        await _dayBookGenericRepository.AddAsync(deduction);

                        var tdsDaybook = new TransFundTd
                        {
                            FundReferenceId = createdTransFund.Id,
                            TdsableAmount = command.Tds.TdsableAmount,
                            Tdsper = command.Tds.Tdsper,
                            PanNo = command.Tds.PanNo,
                            Tds = command.Tds.Tds,

                        };
                        await _tdsGenericRepository.AddAsync(tdsDaybook);
                    }
                    if (command.Deductions != null && command.Deductions.Any())
                    {
                        foreach (var deduction in command.Deductions)
                        {
                            var deductionDaybook = new Daybook
                            {
                                AccountId = deduction.EntityId,
                                Amount = deduction.DeductionAmount,
                                TransType = "CR",
                                FranchiseId = loginId,
                                SessionId = command.SessionId,
                                Status = true,
                                FundReferenceId = createdTransFund.Id,
                                ParentId = createdDayBookCR.Id,
                                StaffId = loginId,
                            };
                            await _dayBookGenericRepository.AddAsync(deductionDaybook);
                        }
                    }

                    if (command.PayType.ToLower() == "loan")
                    {
                        var loanAccount = await _loanRepository1.GetByEntityIdAsync((int)command.EntityId);
                        var data = loanAccount.FirstOrDefault(x => x.Id == command.LoanId);
                        if (data == null)
                        {
                            return BadRequest("Loan not found with the given LoanId for this EntityId.");
                        }
                        var loanAccountDetails = data?.LoanAccountDetails.Where(d => d.Status == false).ToList();
                        if (loanAccountDetails == null || !loanAccountDetails.Any())
                        {
                            return BadRequest("Loan not available on this Entity Id.");
                        }

                        var oneEmipayableAmount = loanAccountDetails.First().PayableAmount ?? 0;
                        int multiples = (int)(command.Amount / oneEmipayableAmount);
                        decimal remainingAmount = (decimal)command.Amount % oneEmipayableAmount;
                        decimal totalPayableAmount = loanAccountDetails.Sum(x => x.PayableAmount ?? 0);
                        if (command.Amount > totalPayableAmount)
                        {
                            return BadRequest($"The total amount to be paid exceeds the remaining payable amount of {totalPayableAmount:F2}.");
                        }

                        if (multiples == 0)
                        {
                            return BadRequest($"want minimum amount to pay {oneEmipayableAmount} ");
                        }

                        if (multiples > loanAccountDetails.Count)
                        {
                            return BadRequest("Not enough unpaid loan account details to cover the specified amount.");
                        }

                        for (int i = 0; i < multiples - 1; i++)
                        {
                            var loanAccountDetail = loanAccountDetails[i];
                            loanAccountDetail.FundReferenceId = transFund.Id;
                            loanAccountDetail.EntityId = command.EntityId;
                            loanAccountDetail.Status = true;
                            await _loanAccountDetailRepository.UpdateAsync(loanAccountDetail.Id, loanAccountDetail);
                        }
                        var remainingLoanAccountDetail = loanAccountDetails[multiples - 1];
                        remainingLoanAccountDetail.FundReferenceId = transFund.Id;
                        remainingLoanAccountDetail.EntityId = command.EntityId;

                        if (remainingAmount > 0)
                        {
                            remainingLoanAccountDetail.LoanAmount = (remainingLoanAccountDetail.LoanAmount ?? 0) + remainingAmount;
                            remainingLoanAccountDetail.PayableAmount = (remainingLoanAccountDetail.PayableAmount ?? 0) + remainingAmount;
                        }

                        remainingLoanAccountDetail.Status = true;
                        await _loanAccountDetailRepository.UpdateAsync(remainingLoanAccountDetail.Id, remainingLoanAccountDetail);

                        var extraLoanAccountDetails = loanAccountDetails.Skip(multiples).ToList();
                        var totalEmiMonth = extraLoanAccountDetails.Count();

                        if (multiples == loanAccountDetails.Count)
                        {
                            data.Status = false;
                            await _loanAccountRepository.UpdateAsync(data.Id, data);
                        }

                        if (remainingAmount > 0)
                        {
                            decimal pendingAmount = 0;
                            var emiMonth = 0;

                            emiMonth = totalEmiMonth;
                            foreach (var extraLoanAccountDetail in extraLoanAccountDetails)
                            {
                                pendingAmount += extraLoanAccountDetail.LoanAmount.Value;
                                await _loanAccountDetailRepository.RemoveAsync(extraLoanAccountDetail);
                            }
                            pendingAmount -= remainingAmount;
                            decimal monthlyInterestRate = (decimal)loanAccountDetails.First().InterestPercentage / 12 / 100;

                            var amountcheckEqual = remainingAmount * monthlyInterestRate * 10;
                            data.PayableAmount = data.PayableAmount - amountcheckEqual;
                            await _loanAccountRepository.UpdateAsync(data.Id, data);



                            decimal OnePlusRPowerN(decimal baseValue, int exponent)
                            {
                                decimal result = 1;
                                for (int i = 0; i < exponent; i++)
                                {
                                    result *= baseValue;
                                }
                                return result;
                            }

                            var onePlusRPowerN = OnePlusRPowerN(1 + monthlyInterestRate, emiMonth);
                            var emi = (pendingAmount * monthlyInterestRate * onePlusRPowerN) / (onePlusRPowerN - 1);
                            var outstandingPrincipal = pendingAmount;

                            for (int i = 0; i < emiMonth; i++)
                            {
                                decimal interestPayment = outstandingPrincipal * monthlyInterestRate;
                                decimal principalPayment = (decimal)emi - interestPayment;
                                outstandingPrincipal -= principalPayment;

                                var newLoanAccountDetail = new LoanAccountDetail
                                {
                                    LoanAccountId = remainingLoanAccountDetail.LoanAccountId,
                                    EmiMonth = i + 1,
                                    LoanAmount = principalPayment,
                                    InterestAmount = interestPayment,
                                    PayableAmount = emi,
                                    InterestPercentage = (decimal)loanAccountDetails.First().InterestPercentage,
                                    StartDate = DateTime.Now,
                                    DueDate = DateTime.Now.AddMonths(1),
                                    Status = false,
                                    EntityId = null,
                                    FundReferenceId = null,
                                };
                                await _loanAccountDetailRepository.AddAsync(newLoanAccountDetail);
                            }

                            var loanAccountRepeat = await _loanRepository1.GetByEntityIdAsync((int)command.EntityId);
                            var dataRepeat = loanAccountRepeat.FirstOrDefault(x => x.Id == command.LoanId);

                            if (dataRepeat != null)
                            {
                                var updatedLoanAccountDetails = dataRepeat.LoanAccountDetails;
                                dataRepeat.PayableAmount = updatedLoanAccountDetails.Sum(x => x.PayableAmount ?? 0);
                                await _loanAccountRepository.UpdateAsync(dataRepeat.Id, dataRepeat);
                            }
                        }
                    }

                    await transaction.CommitAsync();
                    return Ok(transFund.Id);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred: " + ex.Message);
                }
            }

        }


        [HttpPost]
        [Route("generalvoucher")]
        public async Task<ActionResult<string>> CreateGeneralVoucher(CreateGeneralVoucherDto command)
        {
            var loginIdClaim = _contextAccessor.HttpContext.User.FindFirstValue("id");
            int? loginId = null;
            if (!string.IsNullOrEmpty(loginIdClaim))
            {
                loginId = Convert.ToInt32(loginIdClaim);
            }
            var voucherSRNOs = await _voucherSrNoRepository.GetAllAsync();
            var srNo = voucherSRNOs.Select(pd => pd.SrNo).DefaultIfEmpty(0).Max();
            var newsrNo = srNo + 1;
            var voucherTypeGet = await _voucherTypeRepository.GetAllAsync();
            var voucherTypeSelect = voucherTypeGet.FirstOrDefault(m => m.Name.ToLower().Equals("General", StringComparison.OrdinalIgnoreCase));
            if (voucherTypeSelect == null)
            {
                return BadRequest("Voucher type 'General' not found.");
            }
            if (command.DRAccount.HasValue)
            {
                var entity = await _entityGenericRepository.GetByIdAsync(command.DRAccount.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid Entity Id");
                }
            }
            if (command.CRAccount.HasValue)
            {
                var entity = await _entityGenericRepository.GetByIdAsync(command.CRAccount.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid Entity Id");
                }
            }
            if (command.EntityId.HasValue)
            {
                var entity = await _entityGenericRepository.GetByIdAsync(command.EntityId.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid Entity Id");
                }
            }

            var voucherNo = new VoucherSrNo
            {
                SessionId = command.SessionId,
                ClfId = command.EntityId,
                SrNo = newsrNo,
                VoucherTypeId = voucherTypeSelect.Id,
            };
            var createdVoucherNo = await _voucherSrNoRepository.AddAsync(voucherNo);

            var transFund = new TransFund
            {
                EntityId = loginId,
                VoucherNo = createdVoucherNo.Id,
                VoucherType = voucherTypeSelect.Id,
                Remark = command.Remark,
                FranchiseId = command.EntityId,
                TotalAmount = command.TotalAmount,
                EntryDate = command.EntryDate,
                SessionId = command.SessionId,
                Status = true,
            };
            var createdTransFund = await _transFundRepository.AddAsync(transFund);

            var DRdaybook = new Daybook
            {
                AccountId = command.DRAccount,
                Status = true,
                FranchiseId = loginId,
                FundReferenceId = createdTransFund.Id,
                TransType = "DR",
                SessionId = command.SessionId,
                Amount = command.TotalAmount,

            };
            var createdDRDayBook = await _dayBookGenericRepository.AddAsync(DRdaybook);
            var CRdaybook = new Daybook
            {
                AccountId = command.CRAccount,
                Status = true,
                FranchiseId = command.EntityId,
                FundReferenceId = createdTransFund.Id,
                TransType = "CR",
                SessionId = command.SessionId,
                Amount = command.TotalAmount,
                ParentId = createdDRDayBook.Id,
            };
            await _dayBookGenericRepository.AddAsync(CRdaybook);

            return "general Voucher created successfully!";
        }


        [HttpPost]
        [Route("expense-charges")]
        public async Task<ActionResult<string>> CreateExpenseAndCharge(CreateChargeExpenseDto command)
        {
            if (command.AccountId.HasValue)
            {
                var entity = await _entityGenericRepository.GetByIdAsync(command.AccountId.Value);
                if (entity == null)
                {
                    return BadRequest("Invalid Account Id");
                }
            }
            var daybook = new Daybook
            {
                AccountId = command.AccountId,
                Status = true,
                FundReferenceId = command.FundReferenceId,
                TransType = "CR",
                SessionId = command.SessionId,
                Amount = command.Amount,
                FranchiseId = command.FranchiseId
            };
            await _dayBookGenericRepository.AddAsync(daybook);
            return "Charge and Expense created successfully!";
        }
    }
}






