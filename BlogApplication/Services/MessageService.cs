using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace BlogApplication.Services
{
    public class MessageService:IMessageService
    {
        private string _host;
        private int _port;
        private bool _enableSSl;
        private string _username;
        private string _password;
        public MessageService(string host, int port, bool enableSSL, string username, string password)
        {
            _host = host;
            _port = port;
            _enableSSl = enableSSL;
            _username = username;
            _password = password;
        }

        public async Task EmailSenderAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = _enableSSl
            };
            await  client.SendMailAsync(new MailMessage(_username, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            }); 
        }
    }
}
