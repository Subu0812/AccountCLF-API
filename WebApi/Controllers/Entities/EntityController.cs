using AccountCLF.Application.Contract.Entities;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Entities
{
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
        private readonly IMapper _mapper;
        public EntityController(IGenericRepository<Entity> entityGenericRepository, IGenericRepository<ProfileLink> profileGenericRepository,
            IGenericRepository<MasterLogin> masterloginGenericRepository, IMapper mapper,
            IGenericRepository<BasicProfile> basicProfileGenericRepository, IGenericRepository<ContactProfile> contactProfileGenericRepository,
            IGenericRepository<MasterTypeDetail> masterTypeDetailGenericRepository, IGenericRepository<AccountGroup> accountGroupGenericRepository, 
            IGenericRepository<AccountSession> accountSessionGenericRepository)
        {
            _entityGenericRepository = entityGenericRepository;
            _profileGenericRepository = profileGenericRepository;
            _masterloginGenericRepository = masterloginGenericRepository;
            _mapper = mapper;
            _basicProfileGenericRepository = basicProfileGenericRepository;
            _contactProfileGenericRepository = contactProfileGenericRepository;
            _masterTypeDetailGenericRepository = masterTypeDetailGenericRepository;
            _accountGroupGenericRepository = accountGroupGenericRepository;
            _accountSessionGenericRepository = accountSessionGenericRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation(CreateEntityDto entityDto)
        {

            try
            {
                if (entityDto.ContactTypeId != null)
                {
                    var contactTypeId = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.ContactTypeId);
                    if (contactTypeId == null)
                    {
                        return BadRequest("invalid Contact type Id");
                    }
                }
                if(entityDto.AccountTypeId != null)
                {
                    var accountTypeId = await _accountGroupGenericRepository.GetByIdAsync((int)entityDto.AccountTypeId);
                    if (accountTypeId == null)
                    {
                        return BadRequest("invalid Account type Id");
                    }
                }
                if(entityDto.AccountTypeId != null)
                {
                    var accountSessionId = await _accountSessionGenericRepository.GetByIdAsync((int)entityDto.SessionId);
                    if (accountSessionId == null)
                    {
                        return BadRequest("invalid Account Session Id");
                    }
                }
                if(entityDto.ReferenceId != null)
                {
                    var referenceId = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.ReferenceId);
                    if (referenceId == null)
                    {
                        return BadRequest("invalid Reference Id");
                    }
                }
                if(entityDto.StaffId != null)
                {
                    var staffId = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.StaffId);
                    if (staffId == null)
                    {
                        return BadRequest("invalid staff Id");
                    }
                }
                if(entityDto.TypeId != null)
                {
                    var typeId = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.TypeId);
                    if (typeId == null)
                    {
                        return BadRequest("invalid type Id");
                    }
                }
                if(entityDto.Designation != null)
                {
                    var designation = await _masterTypeDetailGenericRepository.GetByIdAsync((int)entityDto.Designation);
                    if (designation == null)
                    {
                        return BadRequest("invalid designation ");
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
                };
                var createdEntity = await _entityGenericRepository.AddAsync(entity);
                var profileLink = new ProfileLink
                {
                    EntityId = createdEntity.Id,
                    FatherName = entityDto.FatherName,
                    MotherName = entityDto.MotherName,
                };
                var createProfileLink = await _profileGenericRepository.AddAsync(profileLink);
                var masterLogin = new MasterLogin
                {
                    EntityId = createdEntity.Id,
                    Password = entityDto.Password,
                    UserName = entityDto.Email
                };
                var createMasterLogin = await _masterloginGenericRepository.AddAsync(masterLogin);
                var basicProfile = new BasicProfile
                {
                    EntityId = createdEntity.Id,
                    Code = entityDto.Code,
                    Name = entityDto.Name,
                    Designation = entityDto.Designation,
                };
                var createBasicProfile = await _basicProfileGenericRepository.AddAsync(basicProfile);
                var contactProfile = new ContactProfile
                {
                    EntityId = createdEntity.Id,
                    Email = entityDto.Email,
                    MobileNo = entityDto.MobileNo,
                    ContactTypeId = entityDto.ContactTypeId,
                };
                await _contactProfileGenericRepository.AddAsync(contactProfile);
                return Ok("Entity Created!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating entity: " + ex.Message);
            }
        }
    }
}
