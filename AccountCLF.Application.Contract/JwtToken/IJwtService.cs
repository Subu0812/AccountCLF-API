using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.JwtToken
{
    public interface IJwtService
    {
        Task<string> GenerateToken(int entityId);
        bool ValidateToken(string token);
    }
}