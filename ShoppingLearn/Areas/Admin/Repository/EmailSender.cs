using System.Net.Mail;
using System.Net;

namespace ShoppingLearn.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("tranthanhdemo@gmail.com", "ozlf lzht nbov nxor")
            };

            return client.SendMailAsync(
                new MailMessage(from: "tranthanhdemo@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
