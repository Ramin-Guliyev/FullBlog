using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using BlogApplication.Models;
using System.Reflection.Metadata;

namespace BlogApplication.Services
{
    public class Smtp2GoMessageService : IMessageService
    {
        public Task EmailSenderAsync(string email, string subject, string htmlMessage)
        {
            var mail = new MailMessage();
            var client = new SmtpClient("mail.smtp2go.com", 2525) //Port 8025, 587 and 25 can also be used.
            {
                Credentials = new NetworkCredential("", ""), // 2Go config
                EnableSsl = true
            };
            mail.From = new MailAddress("Your email address");
            mail.To.Add(email);
            mail.Subject = subject;
            var plainView = AlternateView.CreateAlternateViewFromString(subject, null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html");
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);
            client.Send(mail);
            return Task.CompletedTask;
        }
    }
}
