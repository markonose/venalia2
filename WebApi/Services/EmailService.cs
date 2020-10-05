using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
        }

        public void Send(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("info", from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("mail.eklip.si", 465, true);
            smtp.Authenticate("test@eklip.si", "QfA)EwGni*QY");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
