using AccountCLF.Application.Contract.Entities;
using AccountCLF.Application.Contract.Entities.Logins;
using AccountCLF.Application.Contract.Services.EmailService;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Entities;
using AccountCLF.Data.Repository.MasterTypeDetails;
using AccountCLF.Data.Repository.OTPS;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;


namespace WebApi.Controllers.Accounts.Entities;

[Route("api/entity")]
[ApiController]
public class EntityController : ControllerBase

{
    private readonly AccountClfContext _dbContext;

    private readonly IGenericRepository<Entity> _entityGenericRepository;
    private readonly IGenericRepository<ProfileLink> _profileGenericRepository;
    private readonly IGenericRepository<MasterLogin> _masterloginGenericRepository;
    private readonly IGenericRepository<BasicProfile> _basicProfileGenericRepository;
    private readonly IGenericRepository<ContactProfile> _contactProfileGenericRepository;
    private readonly IGenericRepository<MasterTypeDetail> _masterTypeDetailGenericRepository;
    private readonly IGenericRepository<AccountGroup> _accountGroupGenericRepository;
    private readonly IGenericRepository<AccountSession> _accountSessionGenericRepository;
    private readonly IGenericRepository<Otp> _otpGenericRepository;
    private readonly IGenericRepository<Designation> _designationGenericRepository;
    private readonly IGenericRepository<Location> _locationGenericRepository;
    private readonly IMapper _mapper;
    private readonly IEntityRepository _entityRepository;
    private readonly IEmailAppService _emailAppService;
    private readonly IOtpRepository _otpRepository;
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<AddressDetail> _addressDetailGenericRepository;
    private readonly IGenericRepository<BankDetail> _bankDetailGenericRepository;
    private readonly IMasterTypeRepository _masterTypeRepository;
    private readonly IGenericRepository<DocumentProfile> _documentProfileGenericRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;




    public EntityController(IGenericRepository<Entity> entityGenericRepository, IGenericRepository<ProfileLink> profileGenericRepository,
        IGenericRepository<MasterLogin> masterloginGenericRepository, IGenericRepository<BasicProfile> basicProfileGenericRepository,
        IGenericRepository<ContactProfile> contactProfileGenericRepository, IGenericRepository<MasterTypeDetail> masterTypeDetailGenericRepository,
        IGenericRepository<AccountGroup> accountGroupGenericRepository, IGenericRepository<AccountSession> accountSessionGenericRepository,
        IGenericRepository<Otp> otpGenericRepository, IGenericRepository<Designation> designationGenericRepository, IMapper mapper,
        IEntityRepository entityRepository, IEmailAppService emailAppService, IOtpRepository otpRepository, IConfiguration configuration,
        IGenericRepository<AddressDetail> addressDetailGenericRepository, IGenericRepository<BankDetail> bankDetailGenericRepository,
        IGenericRepository<Location> locationGenericRepository, IMasterTypeRepository masterTypeRepository,
        IGenericRepository<DocumentProfile> documentProfileGenericRepository, IHttpContextAccessor contextAccessor,
        IHttpClientFactory httpClientFactory, AccountClfContext dbContext)
    {
        _entityGenericRepository = entityGenericRepository;
        _profileGenericRepository = profileGenericRepository;
        _masterloginGenericRepository = masterloginGenericRepository;
        _basicProfileGenericRepository = basicProfileGenericRepository;
        _contactProfileGenericRepository = contactProfileGenericRepository;
        _masterTypeDetailGenericRepository = masterTypeDetailGenericRepository;
        _accountGroupGenericRepository = accountGroupGenericRepository;
        _accountSessionGenericRepository = accountSessionGenericRepository;
        _otpGenericRepository = otpGenericRepository;
        _designationGenericRepository = designationGenericRepository;
        _mapper = mapper;
        _entityRepository = entityRepository;
        _emailAppService = emailAppService;
        _otpRepository = otpRepository;
        _configuration = configuration;
        _addressDetailGenericRepository = addressDetailGenericRepository;
        _bankDetailGenericRepository = bankDetailGenericRepository;
        _locationGenericRepository = locationGenericRepository;
        _masterTypeRepository = masterTypeRepository;
        _documentProfileGenericRepository = documentProfileGenericRepository;
        _contextAccessor = contextAccessor;
        _httpClientFactory = httpClientFactory;
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("ledger-account")]
    public async Task<IActionResult> CreateAccountLedger([FromForm] CreateLedgerAccountDto entityDto)
    {
        var accountType = await _accountGroupGenericRepository.GetByIdAsync((int)entityDto.AccountTypeId);
        if (accountType == null)
        {
            return BadRequest("Account Type id is invalid");
        }
        var entity = new Entity
        {
            Name = entityDto.Name,
            AccountTypeId = entityDto.AccountTypeId,
            SessionId = entityDto.SessionId,
            Date = DateTime.UtcNow,
            IsActive = 1,
            Status = 1,
            IsDelete = false,
        };
        await _entityGenericRepository.AddAsync(entity);
        return Ok("Ledger Account Created Successfully!");
    }


    [HttpGet]
    [Route("ledger/dropdown/{accounttypeid}")]
    public async Task<ActionResult<List<GetEntityBankandAccountNumberDto>>> GetLedgerAccountBy(int accounttypeid)
    {
        var entities = await _entityGenericRepository.GetAllAsync();
        var filterdata = entities.Where(e => e.AccountTypeId == accounttypeid).ToList();
        if (!filterdata.Any())
        {
            return BadRequest("No Data Found For this Account Group ");
        }
        var entityDtos = filterdata.Select(e => new GetEntityBankandAccountNumberDto
        {
            EntityId = e.Id,
            Details = e.Name ?? "null"
        }).ToList();

        return Ok(entityDtos);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateEntityDto entityDto,
        [FromForm] List<IFormFile>? ImagePath,
        [FromForm] List<string>? Addresses,
        [FromForm] List<string>? BankDetails,
        [FromForm] List<string>? Documents
        )
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                if (entityDto.Email == null)
                {
                    return BadRequest("Email is Required");
                }
                var allEntities = await _entityRepository.GetAll();
                var emailToCheck = entityDto.Email.ToLower();
                bool emailExists = allEntities.Any(entity =>
                    entity.ContactProfiles.Any(profile => profile.Email.ToLower() == emailToCheck));
                if (emailExists)
                {
                    return BadRequest("Email already exists");
                }
                if (entityDto.ContactTypeId.HasValue)
                {
                    var contactType = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.ContactTypeId);
                    if (contactType == null) return BadRequest("Invalid Contact Type Id");
                }
                var session = await _accountSessionGenericRepository.GetByIdAsync((int)entityDto.SessionId);
                if (session == null) return BadRequest("Invalid Session Id");

                if (entityDto.ReferenceId.HasValue)
                {
                    var reference = await _entityGenericRepository.GetByIdAsync((int)entityDto.ReferenceId);
                    if (reference == null) return BadRequest("Invalid Reference Id");
                }
                if (entityDto.StaffId.HasValue)
                {
                    var staff = await _entityGenericRepository.GetByIdAsync((int)entityDto.StaffId);
                    if (staff == null) return BadRequest("Invalid Staff Id");
                }
                var type = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.TypeId);
                if (type == null) return BadRequest("Invalid Type Id");

                var addressList = new List<CreateAddressDto>();
                foreach (var addressJson in Addresses)
                {
                    var address = JsonConvert.DeserializeObject<CreateAddressDto>(addressJson);
                    if (address != null)
                    {
                        addressList.Add(address);
                    }
                }
                var bankDetailList = new List<CreateBankDetailDto>();
                foreach (var bankDetailJson in BankDetails)
                {
                    var bankDetail = JsonConvert.DeserializeObject<CreateBankDetailDto>(bankDetailJson);
                    if (bankDetail != null)
                    {
                        bankDetailList.Add(bankDetail);
                    }
                }

                var documentList = new List<CreateDocumentDto>();
                foreach (var documentJson in Documents)
                {
                    var document = JsonConvert.DeserializeObject<CreateDocumentDto>(documentJson);
                    if (document != null)
                    {
                        documentList.Add(document);
                    }
                }

                if (bankDetailList != null && bankDetailList.Any())
                {
                    foreach (var bankDetail in bankDetailList)
                    {

                        if (bankDetail.BankId != 0 && bankDetail.BankId != null)
                        {
                            var bank = await _masterTypeDetailGenericRepository.GetByIdAsync(bankDetail.BankId);
                            if (bank == null) return BadRequest("Invalid Bank Id");
                        }
                        if (bankDetail.ParentId.HasValue)
                        {
                            var parentBank = await _bankDetailGenericRepository.GetByIdAsync((int)bankDetail.ParentId);
                            if (parentBank == null) return BadRequest("Invalid Bank Parent Id");
                        }
                    }
                }
                if (addressList != null)
                {
                    foreach (var address in addressList)
                    {
                        if (address.AddressTypeId.HasValue)
                        {
                            var addressType = await _masterTypeDetailGenericRepository.GetByIdAsync((int)address.AddressTypeId);
                            if (addressType == null) return BadRequest("Invalid Address Type Id");
                        }
                        if (address.CityId.HasValue)
                        {
                            var city = await _locationGenericRepository.GetByIdAsync((int)address.CityId);
                            if (city == null) return BadRequest("Invalid City Id");
                        }
                    }
                }

                var entity = new Entity
                {
                    AccountTypeId = 3,
                    SessionId = entityDto.SessionId,
                    TypeId = entityDto.TypeId,
                    StaffId = entityDto.StaffId,
                    ReferenceId = entityDto.ReferenceId,
                    Status = 1,
                    IsActive = 1,
                    Date = entityDto.Date,
                    Name = entityDto.Name,
                    IsDelete = false,
                };
                var createdEntity = await _entityGenericRepository.AddAsync(entity);

                var profileLink = new ProfileLink
                {
                    EntityId = createdEntity.Id,
                    FatherName = entityDto.FatherName,
                    MotherName = entityDto.MotherName,
                };
                await _profileGenericRepository.AddAsync(profileLink);

                var masterLogin = new MasterLogin
                {
                    EntityId = createdEntity.Id,
                    Password = entityDto.Password,
                    UserName = entityDto.Email,
                    Status = 1
                };
                await _masterloginGenericRepository.AddAsync(masterLogin);

                var contactProfile = new ContactProfile
                {
                    EntityId = createdEntity.Id,
                    Email = entityDto.Email,
                    MobileNo = entityDto.MobileNo,
                    ContactTypeId = entityDto.ContactTypeId,
                };
                await _contactProfileGenericRepository.AddAsync(contactProfile);

                var designation = new Designation
                {
                    EntityId = createdEntity.Id,
                    StartDate = DateTime.Now,
                    IsActive = true,
                    IsDefault = true,
                    DesignationId = entityDto.TypeId,
                    ReferenceId = entityDto.ReferenceId,
                };
                var createdDesignation = await _designationGenericRepository.AddAsync(designation);
                var basicProfile = new BasicProfile
                {
                    EntityId = createdEntity.Id,
                    Code = entityDto.EmpCode,
                    Name = entityDto.Name,
                    DesignationId = createdDesignation.Id
                };
                await _basicProfileGenericRepository.AddAsync(basicProfile);

                if (documentList != null && ImagePath.Any())
                {
                    var masterTypeDetail = await _masterTypeRepository.Get();
                    for (int i = 0; i < ImagePath.Count; i++)
                    {
                        var documentMetadata = documentList[i];
                        var imageFile = ImagePath[i];

                        if (documentMetadata.DocType.HasValue)
                        {
                            var docType = await _masterTypeDetailGenericRepository.GetByIdAsync((int)documentMetadata.DocType);
                            if (docType == null) return BadRequest("Invalid Doc Type Id");
                        }
                        if (imageFile == null || imageFile.Length == 0)
                        {
                            return BadRequest("Document file is required.");
                        }

                        var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                        var allowedExtensionsList = masterTypeDetail
                            .Where(m => m.Type.Name.ToLower() == "documentextension")
                            .ToList();
                        var allowedExtensions = allowedExtensionsList.Select(x => x.Name).ToList();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            return BadRequest($"Invalid file type '{fileExtension}'. Allowed types are: {string.Join(", ", allowedExtensions)}.");
                        }

                        var matchedExtension = allowedExtensionsList.FirstOrDefault(x => x.Name.ToLower() == fileExtension);
                        if (matchedExtension != null)
                        {
                            var fileName = Guid.NewGuid().ToString() + fileExtension;
                            var filePath = Path.Combine("wwwroot/PanCard", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            var imageUrl = Path.Combine("PanCard/", fileName);
                            var documentProfile = new DocumentProfile
                            {
                                SrNo=documentMetadata.DocumentSrNo,
                                EntityId = createdEntity.Id,
                                DocType = documentMetadata.DocType,
                                IsActive = 1,
                                InsDate = DateTime.Now,
                                Description = documentMetadata.Description,
                                DocExtensionId = matchedExtension.Id,
                                Path = imageUrl,
                                Name = documentMetadata.DocumentNumber
                            };
                            await _documentProfileGenericRepository.AddAsync(documentProfile);
                        }
                        else
                        {
                            return BadRequest("The uploaded file type is not supported.");
                        }
                    }
                }

                if (bankDetailList != null && bankDetailList.Any())
                {
                    foreach (var bankDetail in bankDetailList)
                    {
                        var newBankDetail = new BankDetail
                        {
                            SrNo = bankDetail.SrNo,
                            BeneficiaryName = bankDetail.BeneficiaryName,
                            AccountNo = bankDetail.AccountNo,
                            Ifsccode = bankDetail.Ifsccode,
                            BankId = bankDetail.BankId,
                            EntityId = createdEntity.Id,
                        };
                        await _bankDetailGenericRepository.AddAsync(newBankDetail);
                    }
                }
                if (addressList != null && addressList.Any())
                {
                    foreach (var address in addressList)
                    {
                        var newAddress = new AddressDetail
                        {
                            AddressTypeId = address.AddressTypeId,
                            CityId = address.CityId,
                            PinCode = address.PinCode,
                            Address = address.Address,
                            LandMark = address.LandMark,
                            EntityId = createdEntity.Id,
                        };
                        await _addressDetailGenericRepository.AddAsync(newAddress);
                    }
                }
                await transaction.CommitAsync();
                return Ok("Entity Created!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating entity: " + ex.Message);
            }
        }
    }


    [HttpPost]
    [Route("api/checklogin")]
    public async Task<ActionResult<int>> Login([FromBody] LoginDto login)
    {
        var masterLogins = await _entityRepository.GetUserByEmail(login.UserName);
        if (masterLogins == null)
        {
            return BadRequest("invalid credential!");
        }
        if (masterLogins.Password != login.Password)
        {
            return BadRequest("incorrect Password!");
        }
        var emailOtp = await _otpRepository.CreateOtp((int)masterLogins.EntityId, "Email");
        var userProfile = masterLogins.Entity.BasicProfiles.FirstOrDefault(bp => bp.EntityId == masterLogins.EntityId);
        if (userProfile == null)
        {
            return BadRequest("User profile not found!");
        }
        string userName = userProfile.Name;
        string subject = " Account CLF Login Verification OTP......";
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var htmlTemplatePath = Path.Combine(wwwrootPath, "mails/otpemail.html");
        var htmlTemplate = System.IO.File.ReadAllText(htmlTemplatePath);
        string emailBody = htmlTemplate.Replace("{{user_name}}", userName)
                                    .Replace("{{username}}", userName)
                                    .Replace("{{otp}}", $"Your Email OTP is: {emailOtp.Otp1}");
        _emailAppService.SendEmail(masterLogins.UserName, subject, emailBody);
        return Ok(masterLogins.EntityId);
    }


    [HttpGet]
    [Route("registration/parent-senior-list/{typeid}")]
    public async Task<ActionResult<List<GetEntityBankandAccountNumberDto>>> GetRegistrationForTypeParent(int typeid)
    {
        var masterTypeDetails = await _masterTypeRepository.GetById(typeid);
        if (masterTypeDetails == null)
            return BadRequest("Entity Type Id invalid");
        if (masterTypeDetails.ParentId != null)
        {
            var entities = await _entityGenericRepository.GetAllAsync();
            var filterdata = entities.Where(e => e.TypeId == masterTypeDetails.ParentId).ToList();

            if (filterdata == null || !filterdata.Any())
                return NotFound("No entities found for the specified ParentId.");

            var entityDtos = filterdata.Select(e => new GetEntityBankandAccountNumberDto
            {
                EntityId = e.Id,
                Details = e.Name + "(" + masterTypeDetails.Parent.Name + ")" ?? e.BasicProfiles.FirstOrDefault(x => x.EntityId == e.Id).Name ?? "Not Available"
            });

            return Ok(entityDtos);
        }

        return BadRequest("ParentId is null.");
    }


    [HttpGet("validate-ifsc/{ifscCode}")]
    public async Task<IActionResult> ValidateIfsc(string ifscCode)
    {
        if (string.IsNullOrEmpty(ifscCode))
        {
            return BadRequest("IFSC code is required.");
        }


        var ifscPattern = new Regex(@"^[A-Z]{4}[0-9]{7}$");
        if (!ifscPattern.IsMatch(ifscCode.ToUpper()))
        {
            return BadRequest("Invalid IFSC code format.");
        }
        try
        {
            var client = _httpClientFactory.CreateClient();

            string apiUrl = $"https://ifsc.razorpay.com/{ifscCode}";
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseContent);

                var bankName = jsonResponse["BANK"]?.ToString();
                var branchName = jsonResponse["BRANCH"]?.ToString();

                if (string.IsNullOrWhiteSpace(bankName) || string.IsNullOrWhiteSpace(branchName))
                {
                    return BadRequest("Invalid IFSC code or details not found.");
                }

                return Ok(new
                {
                    BankName = bankName,
                    BranchName = branchName
                });
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return BadRequest($"Error from IFSC API: {response.StatusCode} - {responseContent}");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error occurred: " + ex.Message);
        }
    }



    //[HttpGet]
    //[Route("detail/bank-name/account-number")]
    //public async Task<ActionResult<List<GetEntityBankandAccountNumberDto>>> GetBankAndAccountNumber()
    //{
    //    var loginIdClaim = _contextAccessor.HttpContext.User.FindFirstValue("id");
    //    int? loginId = null;
    //    if (!string.IsNullOrEmpty(loginIdClaim))
    //    {
    //        loginId = Convert.ToInt32(loginIdClaim);
    //    }
    //    var entities = await _entityRepository.GetAll();

    //    if (entities == null)
    //    {
    //        return NotFound("Data Not Found");
    //    }

    //    var result = entities
    //     .Where(entity => entity.ParentId == loginId && entity.AccountTypeId == 15)
    //     .Select(entity => new GetEntityBankandAccountNumberDto
    //     {
    //         EntityId = entity.Id,
    //         Details = $"{entity.Name}"
    //     })
    //     .ToList();
    //    if (result.Count == 0)
    //    {
    //        return BadRequest("Bank Ledger Account Not Found For this users");
    //    }
    //    return Ok(result);
    //}


    [HttpGet]
    [Route("detail/bank-name/account-number")]
    public async Task<ActionResult<List<GetEntityBankandAccountNumberDto>>> GetBankAndAccountNumber()
    {
        var loginIdClaim = _contextAccessor.HttpContext.User.FindFirstValue("id");
        int? loginId = null;
        if (!string.IsNullOrEmpty(loginIdClaim))
        {
            loginId = Convert.ToInt32(loginIdClaim);
        }
        var entities = await _entityRepository.GetAll();

        if (entities == null)
        {
            return NotFound("Data Not Found");
        }
        var result = entities.Where(entity => entity.BankDetails.Any(x => x.EntityId == loginId))
         .Select(entity => new GetEntityBankandAccountNumberDto
         {
             EntityId = entity.Id,
             Details = entity.BankDetails
                        .Where(b => b.EntityId == loginId)
                                               .Select(b => $"{b.Bank.Code} - {b.AccountNo}")
                        .FirstOrDefault()
         })
         .ToList();
        if (result.Count == 0)
        {
            return BadRequest("Bank Ledger Account Not Found For this users");
        }
        return Ok(result);
    }


    [HttpGet]
    [Route("cash-bank/entity/ledger-dropdown")]
    public async Task<ActionResult<List<GetEntityBankandAccountNumberDto>>> GetCashAndBankDropdown()
    {
        var entities = await _entityRepository.GetAll();
        if (entities == null)
        {
            return NotFound("Data Not Found");
        }
        var filteredEntities = entities
      .Where(x => x.Id == 237 || x.Id == 239)
      .ToList();

        var result = filteredEntities
            .Select(entity => new GetEntityBankandAccountNumberDto
            {
                EntityId = entity.Id,
                Details = $"{entity.Name}"
            })
            .ToList();
        return Ok(result);
    }


    [HttpGet]
    public async Task<ActionResult<List<GetEntityDto>>> Get()
    {
        var entity = await _entityRepository.GetAll();
        if (entity == null)
        {
            return NotFound("Data Not Found");
        }

        var mappedData = _mapper.Map<List<GetEntityDto>>(entity);
        return Ok(mappedData);
    }


    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<GetEntityDto>> GetById(int id)
    {
        var entity = await _entityRepository.GetById(id);
        if (entity == null)
        {
            return NotFound("Data Not Found");
        }
        var mappedData = _mapper.Map<GetEntityDto>(entity);
        foreach (var address in mappedData.AddressDetails)
        {
            var city = entity.AddressDetails.FirstOrDefault()?.City;
            if (city != null)
            {
                address.ParentId = city.Parent?.Id;
                address.GrandParentId = city.Parent?.Parent?.Id;
            }
        }
        return Ok(mappedData);
    }


    [HttpGet]
    [Route("filter/clf-po-shg/dropdown")]
    public async Task<ActionResult<List<GetEntityDto>>> GetCLFPOSHGDropdown()
    {
        var userId = _contextAccessor.HttpContext.User.FindFirstValue("id");

        if (userId == null)
        {
            return Unauthorized("User not authenticated.");
        }

        var entities = await _entityRepository.GetAll();

        if (entities == null)
        {
            return NotFound("Data Not Found");
        }
        var filteredEntities = entities.Where(e => e.ReferenceId.HasValue && e.ReferenceId.Value == int.Parse(userId) && e.AccountTypeId != 15).ToList();

        var mappedData = _mapper.Map<List<GetEntityDto>>(filteredEntities);
        return Ok(mappedData);
    }


    [HttpGet]
    [Route("verifyotp")]
    public async Task<ActionResult<string>> verifyOtp(int entityId, int otp)
    {
        var checkEntity = await _entityGenericRepository.GetByIdAsync(entityId);
        if (checkEntity == null)
        {
            return BadRequest("Incorrect Entity Id");
        }
        var emailOtp = await _otpRepository.VerifyOtp(entityId, otp);
        if (emailOtp == null)
        {
            return BadRequest("invalid otp");
        }
        if (emailOtp.ExpirationTime < DateTime.Now)
        {
            return BadRequest("Otp Experied");
        }
        if (emailOtp.ForOtp != "Email")
        {
            return BadRequest("Email otp required");
        }
        if (emailOtp.IsChecked)
        {
            return BadRequest("otp already checked");
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var basicProfiles = await _entityRepository.GetBasicProfileByEntityId(entityId);
        var username = basicProfiles.Name ?? "testing";
        var roles = await _entityRepository.GetById(entityId);
        if (roles == null)
        {
            return BadRequest("Roles not found for the entity.");
        }

        var roleName = roles.Type?.Name ?? "testing";

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("Id", entityId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roleName)
                }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        emailOtp.IsChecked = true;
        await _otpGenericRepository.UpdateAsync(emailOtp.Id, emailOtp);

        return jwtToken;
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
        var entityAccount = await _entityGenericRepository.GetByIdAsync(id);

        if (entityAccount == null)
        {
            return BadRequest("Invalid ID");
        }
        var referenceCheckData = await _entityGenericRepository.GetAllAsync();
        var referenceCheck = referenceCheckData.FirstOrDefault(x => x.ReferenceId == id);
        if (referenceCheck != null)
        {
            entityAccount.IsDelete = true;

            await _entityGenericRepository.UpdateAsync(id, entityAccount);
            return Ok("This entity is referenced by other data but will still be deleted.");
        }
        entityAccount.IsDelete = true;

        await _entityGenericRepository.UpdateAsync(id, entityAccount);

        return Ok("Ledger Account marked as deleted successfully!");
    }


    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<int>> UpdateLedgerAccount(int id, UpdateEntityDto command)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var entityAccount = await _entityRepository.GetById(id);
            if (entityAccount == null)
            {
                return BadRequest("Invalid Entity ID. Entity not found.");
            }

            entityAccount.Name = command.Name;
            entityAccount.TypeId = command.TypeId;
            entityAccount.Date = command.Date;
            await _entityGenericRepository.UpdateAsync(id, entityAccount);

            var existingBasicProfile = entityAccount.BasicProfiles.FirstOrDefault(x => x.EntityId == entityAccount.Id);
            existingBasicProfile.Name=command.Name;
            existingBasicProfile.Code = command.Code;
            await _basicProfileGenericRepository.UpdateAsync(existingBasicProfile.Id, existingBasicProfile);

            if (command.ProfileLinks != null)
            {
                var existingProfileLink = entityAccount.ProfileLinks.FirstOrDefault(x => x.EntityId == entityAccount.Id);
                if (existingProfileLink != null)
                {
                    existingProfileLink.FatherName = command.ProfileLinks.FatherName;
                    existingProfileLink.MotherName = command.ProfileLinks.MotherName;
                    await _profileGenericRepository.UpdateAsync(existingProfileLink.Id, existingProfileLink);
                }
            }
            if (command.ContactProfiles != null)
            {
                var contactProfile = entityAccount.ContactProfiles.FirstOrDefault(x => x.EntityId == entityAccount.Id);
                if (contactProfile != null)
                {
                    contactProfile.Email = command.ContactProfiles.Email;
                    contactProfile.MobileNo = command.ContactProfiles.MobileNo;
                    await _contactProfileGenericRepository.UpdateAsync(contactProfile.Id, contactProfile);
                }
            }
            if (command.MasterLogins != null)
            {
                var masterLogin = entityAccount.MasterLogins.FirstOrDefault(x => x.EntityId == entityAccount.Id);
                if (masterLogin != null)
                {
                    masterLogin.UserName = command.ContactProfiles.Email;
                    masterLogin.Password = command.MasterLogins.Password;
                    await _masterloginGenericRepository.UpdateAsync(masterLogin.Id, masterLogin);
                }
            }
            await transaction.CommitAsync();
            return Ok(entityAccount.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "An error occurred while updating the ledger account.");
        }
    }


    [HttpPut]
    [Route("addressdetail/{id}")]
    public async Task<ActionResult<int>> UpdateAddress(int id, UpdateAddressDto command)
    {
        var address = await _addressDetailGenericRepository.GetByIdAsync(id);
        if (address == null)
        {
            return BadRequest("Address not found.");
        }

        //address.AddressTypeId = command.AddressTypeId;
        //address.CityId = command.CityId;
        //address.PinCode = command.PinCode;
        //address.Address = command.Address;
        //address.LandMark = command.LandMark;

        _mapper.Map(command, address);
        await _addressDetailGenericRepository.UpdateAsync(id, address);
        return Ok(address.Id);
    }


    [HttpPut]
    [Route("bankdetail/{id}")]
    public async Task<ActionResult<int>> UpdateBankDetail(int id, UpdateBankDetailDto command)
    {
        var bankDetail = await _bankDetailGenericRepository.GetByIdAsync(id);
        if (bankDetail == null)
        {
            return BadRequest("Bank detail not found.");
        }
        //bankDetail.SrNo = command.SrNo;
        //bankDetail.BeneficiaryName = command.BeneficiaryName;
        //bankDetail.AccountNo = command.AccountNo;
        //bankDetail.Ifsccode = command.Ifsccode;
        //bankDetail.ParentId = command.ParentId;
        //bankDetail.BankId = command.BankId;
        var mappedData = _mapper.Map(command, bankDetail);

        await _bankDetailGenericRepository.UpdateAsync(id, mappedData);
        return Ok(bankDetail.Id);
    }


    //[HttpPut]
    //[Route("UpdateDocument/{id}")]
    //public async Task<ActionResult<int>> UpdateDocument(int id, UpdateDocumentDto documentMetadata, IFormFile? ImagePath)
    //{
    //    var entityAccount = await _entityRepository.GetById(id);
    //    if (entityAccount == null)
    //    {
    //        return BadRequest("Invalid Entity ID.");
    //    }

    //    // Validate document metadata
    //    if (documentMetadata == null)
    //    {
    //        return BadRequest("Document metadata is missing.");
    //    }

    //    // Process the document only if ImagePath is provided (not null)
    //    if (ImagePath != null)
    //    {
    //        var masterTypeDetail = await _masterTypeRepository.Get();

    //        // Validate DocType
    //        if (documentMetadata.DocType.HasValue)
    //        {
    //            var docType = await _masterTypeDetailGenericRepository.GetByIdAsync((int)documentMetadata.DocType);
    //            if (docType == null)
    //                return BadRequest("Invalid Doc Type Id.");
    //        }

    //        // Check and validate the file extension
    //        var fileExtension = Path.GetExtension(ImagePath.FileName).ToLower();
    //        var allowedExtensionsList = masterTypeDetail
    //            .Where(m => m.Type.Name.ToLower() == "documentextension")
    //            .ToList();
    //        var allowedExtensions = allowedExtensionsList.Select(x => x.Name).ToList();

    //        if (!allowedExtensions.Contains(fileExtension))
    //        {
    //            return BadRequest($"Invalid file type '{fileExtension}'. Allowed types are: {string.Join(", ", allowedExtensions)}.");
    //        }

    //        // Save the file if the extension is valid
    //        var matchedExtension = allowedExtensionsList.FirstOrDefault(x => x.Name.ToLower() == fileExtension);
    //        if (matchedExtension != null)
    //        {
    //            var fileName = Guid.NewGuid().ToString() + fileExtension;
    //            var filePath = Path.Combine("wwwroot/Documents", fileName);
    //            using (var stream = new FileStream(filePath, FileMode.Create))
    //            {
    //                await ImagePath.CopyToAsync(stream);
    //            }

    //            var imageUrl = Path.Combine("Documents", fileName);

    //            // Update the document profile if it exists, otherwise create a new one
    //            var existingDocumentProfile = await _documentProfileGenericRepository.GetByIdAsync(documentMetadata.Id);
    //            if (existingDocumentProfile != null)
    //            {
    //                existingDocumentProfile.DocType = documentMetadata.DocType;
    //                existingDocumentProfile.Description = documentMetadata.Description;
    //                existingDocumentProfile.Path = imageUrl; // Update with the new file path
    //                existingDocumentProfile.DocExtensionId = matchedExtension.Id;
    //                existingDocumentProfile.Name = documentMetadata.DocumentNumber;
    //                existingDocumentProfile.IsActive = 1;
    //                existingDocumentProfile.InsDate = DateTime.Now;

    //                await _documentProfileGenericRepository.UpdateAsync(existingDocumentProfile.Id, existingDocumentProfile);
    //            }
    //            else
    //            {
    //                var newDocumentProfile = new DocumentProfile
    //                {
    //                    EntityId = id,
    //                    DocType = documentMetadata.DocType,
    //                    Description = documentMetadata.Description,
    //                    Path = imageUrl,
    //                    DocExtensionId = matchedExtension.Id,
    //                    Name = documentMetadata.DocumentNumber,
    //                    IsActive = 1,
    //                    InsDate = DateTime.Now
    //                };

    //                await _documentProfileGenericRepository.AddAsync(newDocumentProfile);
    //            }
    //        }
    //        else
    //        {
    //            return BadRequest("The uploaded file type is not supported.");
    //        }
    //    }
    //    else
    //    {
    //        // ImagePath is null, update only document metadata (no file update)
    //        var existingDocumentProfile = await _documentProfileGenericRepository.GetByIdAsync(documentMetadata.Id);
    //        if (existingDocumentProfile != null)
    //        {
    //            existingDocumentProfile.DocType = documentMetadata.DocType;
    //            existingDocumentProfile.Description = documentMetadata.Description;
    //            existingDocumentProfile.Name = documentMetadata.DocumentNumber;
    //            existingDocumentProfile.IsActive = 1;
    //            existingDocumentProfile.InsDate = DateTime.Now;

    //            await _documentProfileGenericRepository.UpdateAsync(existingDocumentProfile.Id, existingDocumentProfile);
    //        }
    //        else
    //        {
    //            return BadRequest("Document profile not found.");
    //        }
    //    }

    //    return Ok(id);
    //}

    [HttpPut]
    [Route("documentprofile/{id}")]
    public async Task<ActionResult<int>> UpdateDocument(int id, [FromForm] UpdateDocumentDto documentMetadata)
    {
        var entityAccount = await _documentProfileGenericRepository.GetByIdAsync(id);
        if (entityAccount == null)
        {
            return BadRequest("Invalid document ID.");
        }

        var existingDocumentProfile = new DocumentProfile();
        if (documentMetadata.ImagePath != null)
        {
            var masterTypeDetail = await _masterTypeRepository.Get();
            if (documentMetadata.DocType.HasValue)
            {
                var docType = await _masterTypeDetailGenericRepository.GetByIdAsync((int)documentMetadata.DocType);
                if (docType == null)
                    return BadRequest("Invalid Doc Type Id.");
            }
            var fileExtension = Path.GetExtension(documentMetadata.ImagePath.FileName).ToLower();
            var allowedExtensionsList = masterTypeDetail
                .Where(m => m.Type.Name.ToLower() == "documentextension")
                .ToList();
            var allowedExtensions = allowedExtensionsList.Select(x => x.Name).ToList();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest($"Invalid file type '{fileExtension}'. Allowed types are: {string.Join(", ", allowedExtensions)}.");
            }
            var matchedExtension = allowedExtensionsList.FirstOrDefault(x => x.Name.ToLower() == fileExtension);
            if (matchedExtension != null)
            {
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine("wwwroot/PanCard", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await documentMetadata.ImagePath.CopyToAsync(stream);
                }
                var imageUrl = Path.Combine("PanCard", fileName);
                existingDocumentProfile = await _documentProfileGenericRepository.GetByIdAsync(id);
                if (existingDocumentProfile != null)
                {
                    existingDocumentProfile.Path = imageUrl;
                    existingDocumentProfile.DocExtensionId = matchedExtension.Id;
                }
            }
        }
        existingDocumentProfile = await _documentProfileGenericRepository.GetByIdAsync(id);
        if (existingDocumentProfile != null)
        {
            //documentProfileToUpdate.DocType = documentMetadata.DocType;
            //documentProfileToUpdate.Description = documentMetadata.Description;
            //documentProfileToUpdate.Name = documentMetadata.DocumentNumber;
            //documentProfileToUpdate.IsActive = 1;
            //documentProfileToUpdate.InsDate = DateTime.Now;
            var mappedData = _mapper.Map(documentMetadata, existingDocumentProfile);
            await _documentProfileGenericRepository.UpdateAsync(existingDocumentProfile.Id, existingDocumentProfile);
        }
        else
        {
            return BadRequest("Document profile not found.");
        }
        return Ok(id);
    }


    [HttpPut]
    [Route("ledger-account/{id}")]
    public async Task<ActionResult<int>> UpdateLedgerAccount(int id, CreateLedgerAccountDto command)
    {
        var ledgerAccount = await _entityGenericRepository.GetByIdAsync(id);
        if (ledgerAccount == null)
        {
            return BadRequest("invalid id Entity not found");
        }
        if (command.AccountTypeId.HasValue)
        {
            var accountType = await _accountGroupGenericRepository.GetByIdAsync((int)command.AccountTypeId);
            if (accountType == null)
            {
                return BadRequest("Account Type id is invalid");
            }
        }
        ledgerAccount.AccountTypeId = command.AccountTypeId;
        ledgerAccount.SessionId = command.SessionId;
        ledgerAccount.Name = command.Name;
        await _entityGenericRepository.UpdateAsync(id, ledgerAccount);
        return Ok(ledgerAccount.Id);

    }


    [HttpPost]
    [Route("new-reference-parent")]
    public async Task<ActionResult<string>> PostMultipleNewReference(Create_UpdateledgerAccountDto command)
    {
        var reference = await _entityGenericRepository.GetByIdAsync((int)command.ReferenceId);
        if (reference == null)
            return BadRequest("Invalid Reference Id.");

        var entity = await _entityGenericRepository.GetByIdAsync((int)command.EntityId);
        if (entity == null)
            return BadRequest("Invalid Entity Id.");

        try
        {
            var existingDesignation = await _designationGenericRepository.GetAllAsync();

            var filterdata = existingDesignation.FirstOrDefault(d => d.EntityId == command.EntityId && d.IsDefault == true);

            if (filterdata != null)
            {
                filterdata.EndDate = DateTime.Now;
                filterdata.IsDefault = false;

                await _designationGenericRepository.UpdateAsync(filterdata.Id, filterdata);
            }

            var designation = new Designation
            {
                EntityId = command.EntityId,
                ReferenceId = command.ReferenceId,
                StartDate = DateTime.Now,
                IsActive = true,
                IsDefault = true,
            };

            await _designationGenericRepository.AddAsync(designation);
            return Ok("New parent created successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete]
    [Route("ledger-account/{id}")]
    public async Task<ActionResult<string>> DeleteLedgerAccount(int id)
    {
        var ledgerAccount = await _entityGenericRepository.GetByIdAsync(id);
        if (ledgerAccount == null)
        {
            return BadRequest("invalid id");
        }
        await _entityGenericRepository.RemoveAsync(ledgerAccount);
        return Ok("Ledger Account Delete Successfully!");
    }


    private bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5001",
                ValidAudience = "https://localhost:5001",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

}

