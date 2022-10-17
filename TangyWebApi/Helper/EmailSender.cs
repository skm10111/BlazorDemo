﻿using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace TangyWeb_Api.Helper
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("email"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

                using var emailClinet = new SmtpClient();
                emailClinet.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //emailClinet.Authenticate("saurabh.realbotz@gmail.com", "SK@12345");
                emailClinet.Authenticate("email", "password");
                emailClinet.Send(emailToSend);
                emailClinet.Disconnect(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
