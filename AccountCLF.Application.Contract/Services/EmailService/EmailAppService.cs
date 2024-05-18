using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AccountCLF.Application.Contract.Services.EmailService
{
    public class EmailAppService : IEmailAppService
    {
        public void SendEmail(string recipientEmail, string subject, string htmlBody)
        {
            string senderEmail = "academy@codedonor.in";
            string senderPassword = "fjCmXdM7UxXk";

            SmtpClient smtpClient = new SmtpClient("smtppro.zoho.in")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = htmlBody
            };

            try
            {
                smtpClient.Send(mailMessage);

                Console.WriteLine("Email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email could not be sent: " + ex.Message);
            }

        }


    }
}
