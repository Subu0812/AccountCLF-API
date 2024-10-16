using AccountCLF.Application.Contract;
using AccountCLF.Application.Contract.TransFunds.Advances;
using AccountCLF.Application.Contract.TransFunds.Bills;
using AccountCLF.Application.Contract.TransFunds.FundTransPaymentDetails;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Daybooks;
using AccountCLF.Data.Repository.Entities;
using AccountCLF.Data.Repository.LoanAccounts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Controllers.TransFundPaymentDetails
{
    [Route("api/paymentdetails")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly IGenericRepository<TransFundPaymentDetail> _genericRepository;
        private readonly IGenericRepository<TransFund> _transFundGenericRepository;
        private readonly IGenericRepository<Daybook> _dayBookGenericRepository;
        private readonly IGenericRepository<Entity> _entityGenericRepository;
        private readonly IGenericRepository<MasterTypeDetail> _masterTypeDetailGenericRepository;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IGenericRepository<AccountSession> _accountSessionRepository;
        private readonly IGenericRepository<VoucherType> _voucherTypeRepository;
        private readonly IGenericRepository<VoucherSrNo> _voucherSrNoRepository;
        private readonly IGenericRepository<LoanAccount> _loanAccountRepository;
        private readonly IGenericRepository<LoanAccountDetail> _loanAccountDetailRepository;
        private readonly IGenericRepository<LoanTenure> _loanTenureRepository;
        private readonly IGenericRepository<LoanInterest> _loanInterestRepository;
        private readonly IGenericRepository<TransFundBill> _billRepository;
        private readonly IGenericRepository<TransFundBillingDetail> _billDetailRepository;
        private readonly IGenericRepository<TransFundRemark> _remarkGenericRepository;
        private readonly IGenericRepository<TransFundTd> _tdsGenericRepository;
        private readonly ILoanAccountRepository _loanAccountRepository1;
        private readonly IDayBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IEntityRepository _entityRepository;

        public PaymentDetailsController(
            IMapper mapper,
            IGenericRepository<TransFundPaymentDetail> genericRepository,
            IGenericRepository<TransFund> transFundGenericRepository,
            IGenericRepository<Daybook> dayBookGenericRepository,
            IGenericRepository<Entity> entityGenericRepository,
            IGenericRepository<MasterTypeDetail> masterTypeDetailGenericRepository,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<AccountSession> accountSessionRepository,
            IGenericRepository<VoucherType> voucherTypeRepository,
            IGenericRepository<VoucherSrNo> voucherSrNoRepository,
            IDayBookRepository bookRepository,
            IGenericRepository<LoanAccount> loanAccountRepository,
            IGenericRepository<LoanAccountDetail> loanAccountDetailRepository,
            IGenericRepository<LoanTenure> loanTenureRepository,
            IGenericRepository<LoanInterest> loanInterestRepository,
            ILoanAccountRepository loanAccountRepository1,
            IGenericRepository<TransFundBill> billRepository,
            IGenericRepository<TransFundBillingDetail> billDetailRepository,
            IGenericRepository<TransFundRemark> remarkGenericRepository,
            IEntityRepository entityRepository,
            IGenericRepository<TransFundTd> tdsGenericRepository)
        {
            _mapper = mapper;
            _genericRepository = genericRepository;
            _transFundGenericRepository = transFundGenericRepository;
            _dayBookGenericRepository = dayBookGenericRepository;
            _entityGenericRepository = entityGenericRepository;
            _masterTypeDetailGenericRepository = masterTypeDetailGenericRepository;
            _HttpContextAccessor = httpContextAccessor;
            _accountSessionRepository = accountSessionRepository;
            _voucherTypeRepository = voucherTypeRepository;
            _voucherSrNoRepository = voucherSrNoRepository;
            _bookRepository = bookRepository;
            _loanAccountRepository = loanAccountRepository;
            _loanAccountDetailRepository = loanAccountDetailRepository;
            _loanTenureRepository = loanTenureRepository;
            _loanInterestRepository = loanInterestRepository;
            _loanAccountRepository1 = loanAccountRepository1;
            _billRepository = billRepository;
            _billDetailRepository = billDetailRepository;
            _remarkGenericRepository = remarkGenericRepository;
            _entityRepository = entityRepository;
            _tdsGenericRepository = tdsGenericRepository;
        }


        [HttpGet("balance/payment-receipt/ledgerHead/{entityId}")]
        public async Task<ActionResult<List<BalanceDto>>> GetDaybookBalanceandFilterData(int entityId, [FromQuery] int? ledgerHeadId = null)
        {
            var entity = await _entityGenericRepository.GetByIdAsync(entityId);
            if (entity == null)
            {
                return BadRequest("entity id not valid");
            }
            if (ledgerHeadId.HasValue)
            {
                var ledgerheadId = await _masterTypeDetailGenericRepository.GetByIdAsync(ledgerHeadId.Value);
                if (ledgerheadId == null)
                {
                    return BadRequest("invalid Ledger Head Id");
                }
            }
            var daybooksQuery = await _bookRepository.GetAll();
            var filteredDaybooks = daybooksQuery.Where(d => d.FranchiseId == entityId).ToList();

            if (ledgerHeadId.HasValue)
            {
                filteredDaybooks = filteredDaybooks.Where(d => d.FundReference.LedgerHeadId == ledgerHeadId).ToList();
            }
            if (!filteredDaybooks.Any())
            {
                return NotFound("No records found for the given Franchise ID.");
            }

            List<BalanceDto> balanceDtos = new List<BalanceDto>();
            decimal balance = 0;

            foreach (var daybook in filteredDaybooks)
            {
                if (!ledgerHeadId.HasValue)
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
                balanceDtos.Add(new BalanceDto
                {
                    Id = daybook.Id,
                    FranchiseId = daybook.FranchiseId ?? 0,
                    TransType = daybook.TransType.ToString(),
                    Amount = (daybook.Amount?.ToString("F2") + " " + daybook.TransType) ?? "0",
                    Balance = ledgerHeadId.HasValue ? null : (balance >= 0
            ? balance.ToString("F2") + " CR"
            : (-balance).ToString("F2") + " DR"),
                    Date = daybook.FundReference.Date != null ? (DateTime)daybook.FundReference.Date : null
                });
            }
            return Ok(balanceDtos);
        }


        [HttpPost]
        public async Task<ActionResult<int>> CreatePaymentDetail(CreatePaymentDetailsDto command)
        {
            try
            {
                if (command.EntityId.HasValue)
                {
                    var entity = await _entityGenericRepository.GetByIdAsync(command.EntityId.Value);
                    if (entity == null)
                    {
                        return BadRequest("Invalid EntityId");
                    }
                }
                if (command.EntityBankAccountTypeId.HasValue)
                {
                    var entityAccountnumber = await _entityGenericRepository.GetByIdAsync(command.EntityBankAccountTypeId.Value);
                    if (entityAccountnumber == null)
                    {
                        return BadRequest("Invalid EntityAccountNumber Id");
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

                var payMode = new MasterTypeDetail();
                if (command.PaymentModeId.HasValue)
                {
                    payMode = await _masterTypeDetailGenericRepository.GetByIdAsync(command.PaymentModeId.Value);
                    if (payMode == null)
                    {
                        return BadRequest("Invalid PayMode");
                    }
                }

                var accountId = payMode.Name.ToLower() == "cash" ? 239 : payMode.Name.ToLower() == "bank" ? 237 : (int?)null;
                var account = new Entity();
                if (command.AccountId.HasValue)
                {
                    account = await _entityGenericRepository.GetByIdAsync(command.AccountId.Value);
                    if (account == null)
                    {
                        return BadRequest("Invalid AccountId");
                    }
                }

                var masterTypedetails = new MasterTypeDetail();
                if (command.BankId.HasValue)
                {
                    masterTypedetails = await _masterTypeDetailGenericRepository.GetByIdAsync(command.BankId.Value);
                    if (masterTypedetails == null)
                    {
                        return BadRequest("Invalid BankId");
                    }
                }

                var loginIdClaim = _HttpContextAccessor.HttpContext.User.FindFirstValue("id");
                int? loginId = null;
                if (!string.IsNullOrEmpty(loginIdClaim))
                {
                    loginId = Convert.ToInt32(loginIdClaim);
                }
                if (loginId == null && loginId == 0)
                {
                    return BadRequest("Please login first");
                }

                var listMasterType = await _masterTypeDetailGenericRepository.GetAllAsync();
                var ledgerHead = listMasterType.FirstOrDefault(m => m.Name.ToLower().Equals("payment"));
                if (ledgerHead == null)
                {
                    return BadRequest("ledgerHead with name 'payment' not found");
                }

                var voucherSRNOs = await _voucherSrNoRepository.GetAllAsync();
                var srNo = voucherSRNOs.Select(pd => pd.SrNo).DefaultIfEmpty(0).Max();
                var newsrNo = srNo + 1;

                var orderNo = newsrNo.ToString().PadLeft(7, '0');
                var voucherTypeGet = await _voucherTypeRepository.GetAllAsync();
                var voucherTypeSelect = voucherTypeGet.FirstOrDefault(m => m.Name.ToLower().Equals("payment"));

                if (voucherTypeSelect == null)
                {
                    return BadRequest("Voucher Type with name 'payment' not found");
                }

                string transId = "";
                if (masterTypedetails.Name == "Bank")
                {
                    transId = GenerateBankReferenceId();
                }

                var paymentDetail = await _genericRepository.GetAllAsync();
                var maxNo = paymentDetail.Select(pd => pd.MaxNo).DefaultIfEmpty(0).Max();
                var newMaxNo = maxNo + 1;

                var ordermaxNo = newMaxNo.ToString().PadLeft(7, '0');

                var voucherNo = new VoucherSrNo
                {
                    SessionId = command.SessionId,
                    ClfId = command.EntityId,
                    SrNo = newsrNo,
                    VoucherTypeId = voucherTypeSelect.Id,
                };

                var transFund = new TransFund
                {
                    EntityId = loginId,
                    VoucherType = voucherTypeSelect.Id,
                    TotalAmount = command.Amount,
                    EntryDate = command.EntryDate,
                    Date = DateTime.Now,
                    SessionId = command.SessionId,
                    FranchiseId = command.EntityId,
                    StaffId = loginId ?? null,
                    PayMode = command.PaymentModeId,
                    LedgerHeadId = ledgerHead.Id,
                    Status = true,
                    PaymentConfirmDate = DateTime.Now,
                    TaxableAmount = command.BankChargeAmount,
                    SlipUpload = command.SlipUpload
                };

                var daybookDR = new Daybook
                {
                    AccountId = accountId,
                    Amount = command.Amount,
                    TransType = "DR",
                    FranchiseId = loginId??null,
                    SessionId = command.SessionId,
                    Status = true,
                    DemoId = command.DemoId,
                   StaffId= loginId,
                };

                var daybookCR = new Daybook
                {
                    AccountId = accountId,
                    Amount = command.RecieveAmount,
                    TransType = "CR",
                    StaffId = loginId,
                    FranchiseId = command.EntityId,
                    SessionId = command.SessionId,
                    Status = true,
                    DemoId = command.DemoId,
                    
                };

                var paymentDetails = new TransFundPaymentDetail
                {
                    ReciptNo = ordermaxNo,
                    MaxNo = newMaxNo,
                    BankReferenceStatus = "success",
                    TransactionStatus = "success",
                    TransactionId = transId,
                    BankReferenceId = command.BankReferenceId,
                    LedgerId = command.EntityId,
                    PaymentModeId = command.PaymentModeId,
                    BankId = command.BankId,
                    ApplyDate = DateTime.Now,
                    PayDate = DateTime.Now,
                    DaybookId = daybookDR.Id,
                };

                var createdVoucherNo = await _voucherSrNoRepository.AddAsync(voucherNo);

                transFund.VoucherNo = createdVoucherNo.Id;
                var createdTransFund = await _transFundGenericRepository.AddAsync(transFund);

                daybookDR.FundReferenceId = createdTransFund.Id;
                var createdDayBookDR = await _dayBookGenericRepository.AddAsync(daybookDR);
                daybookCR.ParentId = createdDayBookDR.Id;
                daybookCR.FundReferenceId = createdTransFund.Id;
                var createdDayBookCR = await _dayBookGenericRepository.AddAsync(daybookCR);

                if (command.BankChargeAmount.HasValue && payMode.Name.Equals("bank", StringComparison.OrdinalIgnoreCase))
                {
                    var entityBankCharges = await _entityRepository.GetAll();

                    if (entityBankCharges == null)
                    {
                        return BadRequest("Unable to retrieve entities. The list is null.");
                    }
                    var Bankchargeselect = entityBankCharges
                       .Where(e => !string.IsNullOrEmpty(e.Name))
                   .FirstOrDefault(m => m.Name.ToLower().Equals("Bank Charges", StringComparison.OrdinalIgnoreCase));
                    if (Bankchargeselect != null)
                    {
                        var charges = new Daybook
                        {
                            FundReferenceId = createdTransFund.Id,
                            AccountId = Bankchargeselect.Id,
                            Status = true,
                            TransType = "CR",
                            SessionId = command.SessionId,
                            Amount = command.BankChargeAmount,
                            FranchiseId = command.EntityBankAccountTypeId,
                            ParentId = createdDayBookDR.Id,
                            StaffId = loginId,
                        };
                        await _dayBookGenericRepository.AddAsync(charges);
                    }
                    else
                    {
                        return BadRequest("Entity with name 'bank charges' not found.");
                    }
                }

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
                        FranchiseId = command.EntityId,
                        FundReferenceId = createdTransFund.Id,
                        ParentId = createdDayBookDR.Id,
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
                            FranchiseId = command.EntityId,
                            SessionId = command.SessionId,
                            Status = true,
                            FundReferenceId = createdTransFund.Id,
                            ParentId= createdDayBookDR.Id,
                            StaffId = loginId,
                        };
                        await _dayBookGenericRepository.AddAsync(deductionDaybook);
                    }   
                }

                paymentDetails.FundReferenceId = createdTransFund.Id;
                paymentDetails.TransactionAmount = command.Amount;
                paymentDetails.BankReferenceAmount = command.Amount;
                paymentDetails.TransactionAmount=command.Amount;
                paymentDetails.DaybookId=createdDayBookDR.Id;
                await _genericRepository.AddAsync(paymentDetails);

                return transFund.Id;
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("bill")]
        public async Task<ActionResult<int>> CreateFundBill(CreateFundBillDto command)
        {
            var fundreference = await _transFundGenericRepository.GetByIdAsync(command.FundReferenceId);
            if (fundreference == null)
            {
                return BadRequest("invalid Fund Reference Id ");
            }
            var transFundBill = new TransFundBill
            {
                FundReferenceId = command.FundReferenceId,
                BillDate = DateTime.Now,
                BillNo = command.BillNo
            };
            var data = await _billRepository.AddAsync(transFundBill);
            return Ok(data.Id);
        }


        [HttpPost]
        [Route("advance")]
        public async Task<ActionResult<int>> CreateAdvancePayment(CreateAdvancedDto command)
        {
            var fundreference = await _transFundGenericRepository.GetByIdAsync(command.FundReferenceId);
            if (fundreference == null)
            {
                return BadRequest("invalid Fund Reference Id ");
            }
            var transAdvance = new TransFundRemark
            {
                FundReferenceId = command.FundReferenceId,
                Remarks = command.Remarks,
            };
            var data = await _remarkGenericRepository.AddAsync(transAdvance);
            return Ok(data.Id);
        }



        [HttpGet]
        [Route("loaninterest")]
        public async Task<ActionResult<IEnumerable<LoanInterest>>> GetAllLoanInterest()
        {
            var loanInterest = await _loanInterestRepository.GetAllAsync();
            return Ok(loanInterest);
        }


        [HttpGet]
        [Route("loantenure")]
        public async Task<ActionResult<IEnumerable<LoanTenure>>> GetAllLoanTenure()
        {
            var loanTenure = await _loanTenureRepository.GetAllAsync();
            return Ok(loanTenure);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<TransFundPaymentDetail>> GetPaymentDetailById(int id)
        {
            var paymentDetail = await _genericRepository.GetByIdAsync(id);
            if (paymentDetail == null)
            {
                return BadRequest("invalid Payment detail Id!");
            }
            return Ok(paymentDetail);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _genericRepository.GetByIdAsync(id);
            if (paymentDetail == null)
            {
                return BadRequest("invalid Payment detail Id! ");
            }
            await _genericRepository.RemoveAsync(paymentDetail);
            return Ok("Payment Details Deleted Successfully!");
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccountPaymentDetail(int id,UpdateAccountReportDataDto command)
        {
            var dayBooks = await _bookRepository.GetDaybookById(id);
            if (dayBooks == null) 
            {
                return BadRequest("invalid DayBook Id");
            }
            dayBooks.Amount = command.Amount ?? dayBooks.Amount;
            dayBooks.TransType = command.TransType ?? dayBooks.TransType;
            dayBooks.AccountId = command.AccountId ?? dayBooks.AccountId;
            dayBooks.FundReference.TotalAmount = command.Amount ?? dayBooks.FundReference.TotalAmount;
            dayBooks.FundReference.Date = command.PayDate ?? dayBooks.FundReference.Date;
            dayBooks.FundReference.EntryDate = command.PayDate ?? dayBooks.FundReference.EntryDate;
            await _dayBookGenericRepository.UpdateAsync(id, dayBooks);
            return Ok("Daybook Update Successfully!");

        }





        public static string GenerateBankReferenceId()
        {
            string GenerateRandomString(int length, string chars)
            {
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    var data = new byte[length];
                    crypto.GetBytes(data);
                    var result = new StringBuilder(length);
                    foreach (var byteValue in data)
                    {
                        result.Append(chars[byteValue % chars.Length]);
                    }
                    return result.ToString();
                }
            }

            string firstPart = GenerateRandomString(4, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            string thirdPart = GenerateRandomString(2, "0123456789");
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string fifthPart = GenerateRandomString(6, "0123456789");
            string lastPart = GenerateRandomString(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            return $"{firstPart}R{thirdPart}{datePart}{fifthPart}{lastPart}";
        }


        private string GenerateBillNo()
        {

            string GenerateRandomString(int length, string chars)
            {
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    var data = new byte[length];
                    crypto.GetBytes(data);
                    var result = new StringBuilder(length);
                    foreach (var byteValue in data)
                    {
                        result.Append(chars[byteValue % chars.Length]);
                    }
                    return result.ToString();
                }

            }
            string firstPart = GenerateRandomString(4, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            string thirdPart = GenerateRandomString(2, "0123456789");
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string fifthPart = GenerateRandomString(6, "0123456789");
            string lastPart = GenerateRandomString(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            return $"{firstPart}R{thirdPart}{datePart}{fifthPart}{lastPart}";
        }






    }
}