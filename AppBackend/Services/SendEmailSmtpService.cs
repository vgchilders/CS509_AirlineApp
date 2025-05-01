
using System.Net;
using System.Net.Mail;
using AppBackend.Interfaces;

namespace AppBackend.Services
{
    public class SendEmailSmtpSerivce : IEMailService
    {
        public async Task SendEmail(string to, string subject, string body, byte[] attachment = null, string attachmentFileName = null)
        {
             var smtpUser = Environment.GetEnvironmentVariable("MAILERSEND_SMTP_USER");
            var smtpPass = Environment.GetEnvironmentVariable("MAILERSEND_SMTP_PASS");
            var fromEmail = Environment.GetEnvironmentVariable("MAILERSEND_FROM_EMAIL");
            using var client= new SmtpClient("smtp.mailersend.net", 587)
            {
                EnableSsl=true,
                Credentials= new NetworkCredential(smtpUser,smtpPass)
            };
            var message=new MailMessage(fromEmail!,to,subject,body){
                IsBodyHtml=true
            };
            if(attachment!=null && attachmentFileName!=null)
            {
                message.Attachments.Add(new Attachment(new MemoryStream(attachment),attachmentFileName));
            }
             await client.SendMailAsync(message);
        }
    }
}
