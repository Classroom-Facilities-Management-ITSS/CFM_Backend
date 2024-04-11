using ClassroomManagerAPI.Data.Utility;
using MimeKit;
using MailKit.Net.Smtp;

namespace ClassroomManagerAPI.Services
{
	public class MailService : IMailService
	{
		private readonly IConfiguration configuration;

		public MailService(IConfiguration configuration)
        {
			this.configuration = configuration;
		}
        public async Task SendMail(MailRequest mailRequest)
		{
			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(this.configuration["Emailsettings:Email"]);
			email.To.Add(MailboxAddress.Parse(mailRequest.toEmail));
			email.Subject = mailRequest.subject;
			var builder = new BodyBuilder();
			builder.HtmlBody = mailRequest.body;
			email.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			smtp.Connect(this.configuration["Emailsettings:Host"], Int32.Parse(this.configuration["Emailsettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
			smtp.Authenticate(this.configuration["Emailsettings:Email"], this.configuration["Emailsettings:Password"]);
			await smtp.SendAsync(email);
			//throw new NotImplementedException();
		}
	}
}
