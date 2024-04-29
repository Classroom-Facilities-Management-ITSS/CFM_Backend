using ClassroomManagerAPI.Models.Dto;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IAuthRepository
    {
        public Task<bool> Register(AddUserRequestDto user);
        public Task<string> LogIn(AddUserRequestDto user);
        public Task<bool> Active(string token);
        public Task<bool> UpdatePassword(UpdatePasswordRequestDto user);
        public Task<bool> GenerateNewPassword(string email);
    }
}
