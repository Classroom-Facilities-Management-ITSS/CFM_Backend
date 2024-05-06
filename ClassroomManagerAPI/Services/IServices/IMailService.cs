using ClassroomManagerAPI.Common;

namespace ClassroomManagerAPI.Services.IServices
{
    public interface IMailService
    {
        public Task SendMail(MailRequest mailRequest);
    }
}
