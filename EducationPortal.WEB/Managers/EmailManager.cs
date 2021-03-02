using AegisImplicitMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Managers
{
    internal class EmailManager
    {
        public void SendEmail(string body, string subject, string address)
        {
            string mail = "kursovod.asp@ukr.net";
            string pass = "hRnsFjGp7468BdVw";
            string host = "smtp.ukr.net";           

            //Generate Message 
            var mymessage = new MimeMailMessage();
            mymessage.From = new MimeMailAddress(mail);
            mymessage.To.Add(address);
            mymessage.Subject = subject;

            mymessage.Body = body;

            //Create Smtp Client
            var mailer = new MimeMailer(host, 465);
            mailer.User = mail;
            mailer.Password = pass;
            mailer.SslType = SslMode.Ssl;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            mailer.SendMailAsync(mymessage);
        }
    }
}