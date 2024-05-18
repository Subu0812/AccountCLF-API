using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Services.EmailService
{
    public interface IEmailAppService
    {
        public void SendEmail(string recipientEmail, string? subject, string? htmlBody);
    }
}
