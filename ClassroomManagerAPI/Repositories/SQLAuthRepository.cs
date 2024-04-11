using AutoMapper;
using ClassroomManagerAPI.Data;
using ClassroomManagerAPI.Data.Utility;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClassroomManagerAPI.Repositories
{
	public class SQLAuthRepository : IAuthRepository
	{
		private readonly AppDbContext context;
		private readonly IMapper mapper;
		private readonly IConfiguration configuration;
		private readonly IMailService mailService;

		public SQLAuthRepository(AppDbContext context, IMapper mapper, IConfiguration configuration, IMailService mailService)
        {
			this.context = context;
			this.mapper = mapper;
			this.configuration = configuration;
			this.mailService = mailService;
		}

		private string generateToken(dynamic authClaims)
		{
			var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
			var token = new JwtSecurityToken(
				issuer: this.configuration["Jwt:Issuer"],
				audience: this.configuration["Jwt:Audience"],
				expires: DateTime.Now.AddDays(30),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private string decodeToken(String token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var result = tokenHandler.ReadToken(token) as JwtSecurityToken;
			if (result != null)
			{
				return result.Claims.First(claim => claim.Type.Contains("email")).Value;
			}
			return string.Empty;
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

		public async Task<string> LogIn(AddUserRequestDto user)
		{
			var found = this.context.Accounts!.SingleOrDefault(u => u.Email == user.Email);
			if (found == null) return string.Empty;
			if (found.Active && BCryptService.verifyPassword(user.Password, found.Password))
			{
				var authClaims = new List<Claim>()
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Role, found.Role),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
				return generateToken(authClaims);

			}
			return string.Empty;
		}

		public async Task<bool> Register(AddUserRequestDto user)
		{
			var found = this.context.Accounts!.SingleOrDefault(u => u.Email == user.Email);
			if (found == null)
			{
				user.Password = BCryptService.HashPassword(user.Password);
				var newUser = mapper.Map<Account>(user);
				await this.context.Accounts!.AddAsync(newUser);
				await this.context.SaveChangesAsync();
				var authClaims = new List<Claim>()
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};
				var token = generateToken(authClaims);
				var pathHtml = Path.Combine(Directory.GetCurrentDirectory(), "Views/verify.html");
				string htmlContent = File.ReadAllText(pathHtml);
				string replacedHtmlContent = htmlContent
				.Replace("{{token}}", token)
				.Replace("{{email}}", user.Email);
				MailRequest mailRequest = new MailRequest();
				mailRequest.toEmail = user.Email;
				mailRequest.body = replacedHtmlContent;
				mailRequest.subject = "Verify Account";
				try
				{
					await this.mailService.SendMail(mailRequest);
				}
				catch (Exception ex) { throw; }
				return true;
			}
			return false;
		}
	}
}
