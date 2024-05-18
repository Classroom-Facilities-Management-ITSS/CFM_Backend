using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Models.Dto;

namespace ClassroomManagerAPI.Repositories.IRepositories
{
    public interface IAuthRepository
    {
        public Task<Guid> Register(Account user);
        
        public Task<Account> LogIn(Account user);
        /*
        public Task<bool> Active(string token);
        public Task<bool> UpdatePassword(UpdatePasswordRequestDto user);
        public Task<bool> GenerateNewPassword(string email); */
        
    }
}
