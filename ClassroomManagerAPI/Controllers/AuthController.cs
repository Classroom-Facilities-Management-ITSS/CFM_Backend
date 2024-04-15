using AutoMapper;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ClassroomManagerAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository authRepository;
		private readonly IMapper mapper;
		private readonly ILogger<AuthController> logger;

		public AuthController(IAuthRepository authRepository, IMapper mapper, ILogger<AuthController> logger)
        {
			this.authRepository = authRepository;
			this.mapper = mapper;
			this.logger = logger;
		}

		[HttpPost("sign_in")]
		public async Task<IActionResult> AuthLogIn([FromBody]AddUserRequestDto user)
		{
			var token = await this.authRepository.LogIn(user);
			if (string.IsNullOrEmpty(token)) return BadRequest(new
			{
				status = false,
				message = "Email or password isn't correct"
			});
			return Ok(new
			{
				status = true,
				message = "Login successfully",
				token
			});

		}

		[HttpPost("sign_up")]
		public async Task<IActionResult> AuthRegister([FromBody] AddUserRequestDto user)
		{
			if (await this.authRepository.Register(user)) return Ok(new
			{
				status = true,
				message = "Register successfully"
			});
			return BadRequest(new
			{
				status = false,
				message = "Account is existing"
			});
		}

		[HttpGet("active")]
		public async Task<IActionResult> AuthActive(string token)
		{
			if (await this.authRepository.Active(token)) return Ok(new
			{
				status = true,
				message = "Active your account successfully"
			});
			return BadRequest(new
			{
				status = false,
				message = "Active account fail"
			});
		}

		/*
		[HttpPost("upload")]
		public async Task<IActionResult> UploadProduct([FromForm] ProductDTO product)
		{
			try
			{
				var fileName = product.image.FileName;
				string pattern = @"\.(jpg|jpeg|png|gif)$";
				if (Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase))
				{
					string newFileName = Guid.NewGuid().ToString() + "_" + fileName;
					string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Public", newFileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						product.image.CopyTo(stream);
					}
					return Ok(new
					{
						status = true,
						image_name = newFileName,
						message = "Create successfully"
					});
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return BadRequest(new
			{
				status = false,
				message = "Has Error"
			});
		}
		*/

		/*
		 [HttpGet("{name}")]
        public async Task<IActionResult> GetImage(string name)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Public", name);
            if (!System.IO.File.Exists(imagePath)) return NotFound(new
            {
                status = false,
                message = "Image name is not correct or isn't existing!"
            });
            var image = System.IO.File.OpenRead(imagePath);
            return File(image, "image/jpeg");
        }
		 */
		[HttpPut("update_password")]
		public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDto updatePasswordRequestDto)
		{
			var result = await this.authRepository.UpdatePassword(updatePasswordRequestDto);
			if (result)
			{
				return Ok(new
				{
					status = true,
					message = "Password Updated Successfully"
				});
			} else
			{
				return BadRequest(new
				{
					status = false,
					message = "Unable to update password."
				});
			}
		}

		[HttpPost("forgot_password")]
		public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequestDto forgotPasswordRequestDto)
		{
			var result = await this.authRepository.GenerateNewPassword(forgotPasswordRequestDto.Email);
			if (result == false)
			{
				return BadRequest(new
				{
					status = false,
					message = "Unable to update password"
				});
			} else
			{
				return Ok(new
				{
					status = true,
					message = "Password successfully generated, please check your email"
				});
			}
			
		}
		// Admin listing users
		[HttpGet("users")]
		public async Task<IActionResult> GetAllAsync()
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				var usersList = await this.authRepository.GetAllAccountsAsync();
				var usersDto = this.mapper.Map<List<AccountDto>>(usersList);

				this.logger.LogInformation($"Finished get all request with data: {JsonSerializer.Serialize(usersList)}");
				return Ok(usersDto);
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Admin searching user by Id
		[HttpGet("users/{id:Guid}")]
		public async Task<IActionResult> GetByIdAsync([FromRoute]Guid id)
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				var user = await this.authRepository.GetAccountByIdAsync(id);
				if (user == null)
				{
					return NotFound();
				}
				var userDto = this.mapper.Map<AccountDto>(user);

				this.logger.LogInformation($"Finished get by id request with data: {JsonSerializer.Serialize(user)}");
				return Ok(userDto);
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Admin searching user by Email
		[HttpGet("users/GetByEmail/{id:Guid}")]
		public async Task<IActionResult> GetByEmailAsync([FromRoute]Guid id)
		{
			try
			{
				// TODO: Fix the param to be Email instead of id 
				//_logger.LogInformation("GetByEmailAsync was invoked");
				var userById = await this.authRepository.GetAccountByIdAsync(id);
				if (userById == null)
				{
					return NotFound();
				}
				
				var userByEmail = await this.authRepository.GetAccountByEmailAsync(userById.Email);
				var userDto = this.mapper.Map<AccountDto>(userByEmail);

				this.logger.LogInformation($"Finished get by email request with data: {JsonSerializer.Serialize(userByEmail)}");
				return Ok(userDto);
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Admin update user
		[HttpPut("users/{id:Guid}")]
		public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, AccountDto accountDto)
		{
			try
			{
				var account = this.mapper.Map<Account>(accountDto);
				account = await this.authRepository.UpdateAsync(id, account);

				if (account == null)
				{
					return NotFound();
				}
				return Ok(this.mapper.Map<Account>(account));
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Admin delete an user
		[HttpDelete("users/{id:Guid}")]
		public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
		{
			try
			{
				var account = await this.authRepository.DeleteAsync(id);
				if (account == null)
				{
					return NotFound();
				}

				return Ok(this.mapper.Map<AccountDto>(account));
			}
			catch (Exception ex)
			{
				this.logger.LogError(ex, ex.Message);
				throw;
			}
		}
	}
}
