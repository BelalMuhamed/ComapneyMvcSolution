using CompaneyMvcDAL.Models;
using System;
using System.Net;
using System.Net.Mail;

namespace CompaneyMvcPL.Helper
{
    public static class EmailSttings
    {
        public static void  SendEmail(Email email)
        {
            var Client =new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials=new NetworkCredential("basalbelal25@gmail.com", "uidvxqwcwmwccqeo");
            Client.Send("basalbelal25@gmail.com", email.To, email.Subject, email.Body);
        }

      
    }
}
