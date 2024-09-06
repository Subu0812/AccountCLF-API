using AccountCLF.Application.Contract.Entities;
using AccountCLF.Application.Contract.Entities.Logins;
using AccountCLF.Application.Contract.Locations;
using AccountCLF.Application.Contract.Services.EmailService;
using AccountCLF.Data;
using AccountCLF.Data.Repository.Entities;
using AccountCLF.Data.Repository.MasterTypeDetails;
using AccountCLF.Data.Repository.OTPS;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApi.Controllers.Accounts.Entities;

[Route("api/entity")]
[ApiController]
public class EntityController : ControllerBase
{
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


    public EntityController(IGenericRepository<Entity> entityGenericRepository, IGenericRepository<ProfileLink> profileGenericRepository,
        IGenericRepository<MasterLogin> masterloginGenericRepository, IGenericRepository<BasicProfile> basicProfileGenericRepository,
        IGenericRepository<ContactProfile> contactProfileGenericRepository, IGenericRepository<MasterTypeDetail> masterTypeDetailGenericRepository,
        IGenericRepository<AccountGroup> accountGroupGenericRepository, IGenericRepository<AccountSession> accountSessionGenericRepository,
        IGenericRepository<Otp> otpGenericRepository, IGenericRepository<Designation> designationGenericRepository, IMapper mapper,
        IEntityRepository entityRepository, IEmailAppService emailAppService, IOtpRepository otpRepository, IConfiguration configuration,
        IGenericRepository<AddressDetail> addressDetailGenericRepository, IGenericRepository<BankDetail> bankDetailGenericRepository,
        IGenericRepository<Location> locationGenericRepository, IMasterTypeRepository masterTypeRepository,
        IGenericRepository<DocumentProfile> documentProfileGenericRepository, IHttpContextAccessor contextAccessor)
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
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateEntityDto entityDto)
    {
        try
        {
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
            if (entityDto.AccountTypeId.HasValue)
            {
                var accountType = await _accountGroupGenericRepository.GetByIdAsync((int)entityDto.AccountTypeId);
                if (accountType == null) return BadRequest("Invalid Account Type Id");
            }
            if (entityDto.SessionId.HasValue)
            {
                var session = await _accountSessionGenericRepository.GetByIdAsync((int)entityDto.SessionId);
                if (session == null) return BadRequest("Invalid Session Id");
            }
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
            if (entityDto.TypeId.HasValue)
            {
                var type = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.TypeId);
                if (type == null) return BadRequest("Invalid Type Id");
            }
            if (entityDto.DesignationId.HasValue)
            {
                var designation = await _designationGenericRepository.GetByIdAsync((int)entityDto.DesignationId);
                if (designation == null) return BadRequest("Invalid Designation Id");
            }
            if (entityDto.BankDetails != null && entityDto.BankDetails.Any())
            {
                foreach (var bankDetail in entityDto.BankDetails)
                {
                    if (bankDetail.PaymentModeId != 0 && bankDetail.PaymentModeId != null)
                    {
                        var paymentMode = await _masterTypeDetailGenericRepository.GetByIdAsync(bankDetail.PaymentModeId);
                        if (paymentMode == null) return BadRequest("Invalid Payment Mode Id");
                    }

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

            if (entityDto.Addresses != null && entityDto.Addresses.Any())
            {
                foreach (var address in entityDto.Addresses)
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
                AccountTypeId = entityDto.AccountTypeId,
                SessionId = entityDto.SessionId,
                TypeId = entityDto.TypeId,
                StaffId = entityDto.StaffId,
                ReferenceId = entityDto.ReferenceId,
                Status = entityDto.Status,
                IsActive = entityDto.IsActive,
                Date = entityDto.Date,
                Name=entityDto.Name,
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

            var basicProfile = new BasicProfile
            {
                EntityId = createdEntity.Id,
                Code = entityDto.Email,
                Name = entityDto.Name,
                DesignationId = entityDto.DesignationId
            };
            await _basicProfileGenericRepository.AddAsync(basicProfile);

            var contactProfile = new ContactProfile
            {
                EntityId = createdEntity.Id,
                Email = entityDto.Email,
                MobileNo = entityDto.MobileNo,
                ContactTypeId = entityDto.ContactTypeId,
            };
            await _contactProfileGenericRepository.AddAsync(contactProfile);


            var masterTypeDetail = await _masterTypeRepository.Get();
            var imageFile = entityDto.Path;

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

                // Get all allowed extensions from masterTypeDetail
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

                    var imageUrl = Path.Combine( "PanCard", fileName);

                    var documentProfile = new DocumentProfile
                    {
                        EntityId = createdEntity.Id,
                        DocType = 36,
                        IsActive = 1,
                        InsDate = DateTime.Now,
                        Description = entityDto.Description,
                        DocExtensionId = matchedExtension.Id,
                        Path = imageUrl,
                    };
                    await _documentProfileGenericRepository.AddAsync(documentProfile);

                }
                else
                {

                    throw new Exception("The uploaded file type is not supported.");
                }
            }


            if (entityDto.BankDetails != null && entityDto.BankDetails.Any())
            {
                foreach (var bankDetail in entityDto.BankDetails)
                {
                    var newBankDetail = new BankDetail
                    {
                        SrNo = bankDetail.SrNo,
                        BeneficiaryName = bankDetail.BeneficiaryName,
                        AccountNo = bankDetail.AccountNo,
                        Ifsccode = bankDetail.Ifsccode,
                        BankId = bankDetail.BankId,
                        PaymentModeId = bankDetail.PaymentModeId,
                        EntityId = createdEntity.Id,
                    };
                    await _bankDetailGenericRepository.AddAsync(newBankDetail);
                }
            }

            if (entityDto.Addresses != null && entityDto.Addresses.Any())
            {
                foreach (var address in entityDto.Addresses)
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

            return Ok("Entity Created!");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating entity: " + ex.Message);
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

        var result = entities
         .Where(entity => entity.ParentId == loginId)
         .Select(entity =>  new GetEntityBankandAccountNumberDto
         {
             EntityId = entity.Id,
             Details = $"{entity.Name}"
         })
         .ToList();
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
      .Where(x => x.Id == 63 || x.Id == 65)
      .ToList();

        var result = filteredEntities
            .Select(entity =>  new GetEntityBankandAccountNumberDto
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
        var filteredEntities = entities.Where(e => e.ParentId.HasValue && e.ParentId.Value == int.Parse(userId)&&e.AccountTypeId!=15).ToList();

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

