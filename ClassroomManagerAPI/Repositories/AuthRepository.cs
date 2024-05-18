using AutoMapper;
using ClassroomManagerAPI.Common;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Auth;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories.IRepositories;
using ClassroomManagerAPI.Services;
using ClassroomManagerAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassroomManagerAPI.Repositories
{
    public class AuthRepository : BaseRepository<Account> , IAuthRepository 
	{
		private readonly AppDbContext _context;

		public AuthRepository(AppDbContext context) : base(context)
        {
			_context = context;
		}
		public async Task<bool> Active(string email)
		{
			var found = await _context.Accounts!.SingleOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);
			if (found == null) return false;
			found.Active = true;
			_context.Accounts!.Update(found);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return true;
		}

		public async Task<Account> LogIn(Account account)
		{
			ArgumentNullException.ThrowIfNull(account, "Account");
			return _context.Accounts!.SingleOrDefault(u => u.Email == account.Email && !u.IsDeleted);
		} 

		public async Task<Guid> Register(Account account)
		{
			ArgumentNullException.ThrowIfNull(account, "newAccount");

			var found = _context.Accounts!.SingleOrDefault(u => u.Email == account.Email && !u.IsDeleted);
			if (found == null)
			{
				var created =  await _context.Accounts!.AddAsync(account);
				await _context.SaveChangesAsync(); 
				return created.Entity.Id;
			}
			return Guid.Empty;
		}
		
		public async Task<Account> GetByEmailAsync(string email)
		{
			ArgumentNullException.ThrowIfNullOrEmpty(email, "email");
			return await _context.Accounts!.SingleOrDefaultAsync(u => u.Email == email && u.Active).ConfigureAwait(false);
		}
		
		public async Task<bool> GenerateNewPassword(string email)
		{
			// Check if user exists
			var user = this._context.Accounts!.SingleOrDefault(u => u.Email == email);
			// if email exists, regenerate a new password for the user
			if (user == null) return false;
			if (user.Active)
			{
				_context.Update(user);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}	
	}
}
