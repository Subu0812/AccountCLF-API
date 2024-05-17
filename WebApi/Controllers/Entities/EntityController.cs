using AccountCLF.Application.Contract.Entities;
using AccountCLF.Application.Contract.Locations;
using AccountCLF.Data;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        public EntityController(IGenericRepository<Entity> entityGenericRepository, IGenericRepository<ProfileLink> profileGenericRepository, IGenericRepository<MasterLogin> masterloginGenericRepository, IMapper mapper, IGenericRepository<BasicProfile> basicProfileGenericRepository, IGenericRepository<ContactProfile> contactProfileGenericRepository)
        {
            _entityGenericRepository = entityGenericRepository;
            _profileGenericRepository = profileGenericRepository;
            _masterloginGenericRepository = masterloginGenericRepository;
            _mapper = mapper;
            _basicProfileGenericRepository = basicProfileGenericRepository;
            _contactProfileGenericRepository = contactProfileGenericRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation(CreateEntityDto entityDto)
        {
            try
            {
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
                    EntityId= createdEntity.Id,
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
