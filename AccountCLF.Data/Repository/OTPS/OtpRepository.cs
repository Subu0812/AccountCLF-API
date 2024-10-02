using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AccountCLF.Data.Repository.OTPS
{
    public class OtpRepository : IOtpRepository
    {
        private readonly AccountClfContext _context;
        public OtpRepository(AccountClfContext context)
        {
            _context = context;
        }
        public async Task<Otp> CreateOtp(int enittyId,string forOtp)
        {
            int otp = GenerateUniqueOTP();

            DateTime creationTime = DateTime.Now;

            DateTime expiryTime = creationTime.AddDays(1);
            var otpEntity = new Otp
            {
                EntityId = enittyId,
                Otp1 = otp,
                CreatedDate = creationTime,
                ExpirationTime = expiryTime,
                IsChecked = false,
                ForOtp = forOtp
            };
             _context.Otps.Add(otpEntity);
           await _context.SaveChangesAsync();
            return otpEntity;
        }

        public Task<Otp> GetLastOtpByUserId(int enittyId)
        {
            throw new NotImplementedException();
        }

        public Task<Otp> GetOtpByUserId(int enittyId)
        {
            throw new NotImplementedException();
        }

        public async Task<Otp> VerifyOtp(int enittyId, int otp)
        {
            var getData = await _context.Otps.Include(x => x.Entity)
              .Where(x =>  x.EntityId == enittyId&&x.IsChecked!=true)
              .ToListAsync();

            var data = getData.OrderBy(x=>x.Id).Last();
            if (data == null)
            {
                return null;
            }
            if (data.Otp1! != otp) 
            {
                return null;
            }
            return data;
        }
        private int GenerateUniqueOTP()
        {
            Random random = new Random();
            int otpValue = random.Next(10000, 99999);

            while (_context.Otps.Any(o => o.Otp1 == otpValue))
            {
                otpValue = random.Next(10000, 99999);
            }
            return otpValue;
        }
    }
}
