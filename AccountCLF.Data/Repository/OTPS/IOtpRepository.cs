using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Data.Repository.OTPS
{
    public interface IOtpRepository
    {
        Task<Otp> CreateOtp(int entitytId,string forOtp);
        Task<Otp> GetLastOtpByUserId(int entitytId);
        Task<Otp> GetOtpByUserId(int entitytId);
        Task<Otp> VerifyOtp(int entitytId, int otp);
    }
}
