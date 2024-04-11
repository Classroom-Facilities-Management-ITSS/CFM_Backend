using ClassroomManagerAPI.Data.Utility;

namespace ClassroomManagerAPI.Services
{
	public interface IMailService
	{
		public Task SendMail(MailRequest mailRequest);
	}
}
