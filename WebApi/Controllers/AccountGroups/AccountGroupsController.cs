using AccountCLF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.AccountGroups
{
    [Route("api/accountgroup")]
    [ApiController]
    public class AccountGroupsController : ControllerBase
    {
        private readonly IGenericRepository<AccountGroup> _genericRepository;
        public AccountGroupsController(IGenericRepository<AccountGroup> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<AccountGroup>> Get()
        {
            var accountGroup = await _genericRepository.GetAllAsync();
            return accountGroup;
        }
    }
}
