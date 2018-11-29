using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace FIT5032_fitness.Models
{
    public class EmailSender
    {
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "Fitness First Booking Confirmation");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}