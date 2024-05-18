using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Services.WhatsappService
{
    public interface IWhatsappAppService
    {
        Task<string> SendWhatsAppMessage(long toNumber, string message);
    }
}
