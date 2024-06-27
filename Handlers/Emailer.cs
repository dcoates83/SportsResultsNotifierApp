using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SportsResultsNotifierApp.Handlers
{

    public class Emailer
    {
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string ReceiverEmail { get; set; }

        public Emailer(string senderEmail, string senderPassword, string receiverEmail)
        {
            SenderEmail = senderEmail;
            SenderPassword = senderPassword;
            ReceiverEmail = receiverEmail;
        }

        public void SendEmail(string subject, string body)
        {
            using MailMessage email = new();
            email.From = new MailAddress(SenderEmail);
            email.To.Add(ReceiverEmail);
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            using SmtpClient smtp = new("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

    }


}