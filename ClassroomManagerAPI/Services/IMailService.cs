using ClassroomManagerAPI.Models;

namespace ClassroomManagerAPI.Services
{
    public interface IMailService
	{
		public Task SendMail(MailRequest mailRequest);
	}
}
