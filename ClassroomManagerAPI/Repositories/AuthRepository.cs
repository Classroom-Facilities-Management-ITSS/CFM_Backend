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
    public class AuthRepository : IAuthRepository 
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly IMailService _mailService;
		private readonly IBCryptService _bCryptService;

		public AuthRepository(AppDbContext context, IMapper mapper, IConfiguration configuration, IMailService mailService, IBCryptService bCryptService)
        {
			_context = context;
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
		/*
		public async Task<bool> UpdatePassword(UpdatePasswordRequestDto user)
		{

			// TODO: return different message for different errors
			var found = this.context.Accounts!.SingleOrDefault(u => u.Email == user.Email);
			if (found == null) return false;
			if (user.NewPassword != user.ConfirmPassword) return false;
			if (!bCryptService.verifyPassword(user.OldPassword, found.Password)) return false;
			if (found != null && found.Active)
			{
				var password = bCryptService.HashPassword(user.NewPassword);
				found.Password = password;
				this.context.Accounts.Update(found);
				await this.context.SaveChangesAsync();

				return true;
			}

			return false;	
		}
		
		public async Task<bool> GenerateNewPassword(string email)
		{
			// TODO: return different message for different errors
			// Check if user exists
			var user = this.context.Accounts!.SingleOrDefault(u => u.Email == email);
			// if email exists, regenerate a new password for the user
			if (user == null) return false;
			if (user.Active)
			{
				var chars = configuration["resetKey"];
				var random = new Random();
				var passwordChars = new char[8];

				for(int i = 0; i < passwordChars.Length; i++)
				{
					passwordChars[i] = chars[random.Next(chars.Length)];
				}

				string passwordString = new string(passwordChars);

				var password = bCryptService.HashPassword(passwordString);
				user.Password = password;
				this.context.Update(user);
				await this.context.SaveChangesAsync();

				// send the new password to the email 
				var mailRequest = new MailRequest
				{
					toEmail = email,
					subject = "Your new Password",
					body = $"Use this password to login and change your password: {passwordString} "
				};

				try
				{
					await this.mailService.SendMail(mailRequest);
				}
				catch (Exception ex)
				{
					// Handle exception (e.g. logger)
					throw;
				}

				return true;
			}
			return false;
		}

		 public async Task<bool> Active(string token)
		{
			var email = decodeToken(token);
			if (string.IsNullOrEmpty(email)) return false;
			var found = await this.context.Accounts!.SingleOrDefaultAsync(u => u.Email == email);
			if (found == null) return false;
			found.Active = true;
			this.context.Accounts!.Update(found);
			await this.context.SaveChangesAsync();
			return true;
		}
		*/
	}
}
