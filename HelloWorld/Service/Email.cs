using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Windows.Forms;

namespace HelloWorld.Service
{
    public class Email
    {
        private SmtpClient _smtp;
        private const string From = "skyhat@mail.ru";

        public Email(string to, string subject, string body)
        {
            InitialClient("skyhat@mail.ru", "mazahaka");

            var message = new MailMessage(From, to, subject, body) {IsBodyHtml = true};

            try
            {
                _smtp.Send(message);
            }
            catch (SmtpException e)
            {
                MessageBox.Show("Ошибка!"+e);
            }
        }

        private void InitialClient(string email, string password)
        {
            _smtp = new SmtpClient("smtp.mail.ru", 25) {Credentials = new NetworkCredential(email, password)};
        }

    }
}