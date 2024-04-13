using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ClassroomManagerAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository authRepository;

		public AuthController(IAuthRepository authRepository)
        {
			this.authRepository = authRepository;
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

	}
}
