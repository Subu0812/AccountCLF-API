using AccountCLF.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApi.Controllers.Accounts.AccountSessions
{
    [Route("api/accountsession")]
    [ApiController]
    public class AccountSessionsController : ControllerBase
    {
        private readonly IGenericRepository<AccountSession> _repository;
        public AccountSessionsController(IGenericRepository<AccountSession> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IEnumerable<AccountSession>> GetMasterTypeDetails()
        {
            var accountSessions = await _repository.GetAllAsync();
            return accountSessions;
        }
    }
}
