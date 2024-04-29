using AutoMapper;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/classroom")]
    [ApiController]
	public class ClassroomAPIController : ControllerBase
	{
		private readonly AppDbContext context;
		private ResponseDto response;
		private readonly IMapper mapper;
		private readonly ILogger<ClassroomAPIController> logger;

		public ClassroomAPIController(AppDbContext context, IMapper mapper, ILogger<ClassroomAPIController> logger)
        {
			this.context = context;
			this.response = new ResponseDto();
			this.mapper = mapper;
			this.logger = logger;
		}

		// Get all classrooms
		[HttpGet, Route("classrooms")]
		public ResponseDto GetAll()
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				IEnumerable<Classroom> objList = context.Classrooms.ToList();
				response.Result = mapper.Map<IEnumerable<ClassroomDto>>(objList);
				//_logger.LogInformation($"Finished get all request with data: {JsonSerializer.Serialize(objList)}");
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message;
				logger.LogError(ex, ex.Message);
				throw;
			}
			return response;
		}

		// Get by number
		[HttpGet]
		[Route("classrooms/GetByName/{number}")]
		public ResponseDto GetByName(string number)
		{
			try
			{
				Classroom obj = context.Classrooms.FirstOrDefault(u => u.ClassNumber.ToLower() == number.ToLower());
				if (obj == null)
				{
					response.IsSuccess = false;
				}
				response.Result = mapper.Map<ClassroomDto>(obj);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message;
				logger.LogError(ex, ex.Message);
				throw;
			}
			return response;
		}

		// Create classroom
		[HttpPost]
		public ResponseDto Create([FromBody] AddClassroomRequestDto addClassroomRequestDto)
		{
			try
			{
				Classroom obj = mapper.Map<Classroom>(addClassroomRequestDto);
				context.Classrooms.Add(obj);
				context.SaveChanges();
				response.Result = mapper.Map<ClassroomDto>(obj);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message;
				logger.LogError(ex, ex.Message);
			}
			return response;
		}

		// Update a classroom
		[HttpPut]
		public ResponseDto Update([FromBody] ClassroomDto classroomDto)
		{
			try
			{
				Classroom existingClassroom = context.Classrooms.FirstOrDefault(c => c.ClassNumber == classroomDto.ClassNumber);
				if (existingClassroom == null)
				{
					// Handle case where the classroom with the specified Id is not found
					// Return appropriate response or throw exception
				}
				mapper.Map(classroomDto, existingClassroom);
				context.Classrooms.Update(existingClassroom);
				context.SaveChanges();
				response.Result = mapper.Map<ClassroomDto>(existingClassroom);
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message;
				logger.LogError(ex, ex.Message);
			}
			return response;
		}

		// Remove a classroom
		[HttpDelete]
		[Route("classrooms/{number}")]
		public ResponseDto Delete(string number)
		{
			try
			{
				Classroom obj = context.Classrooms.First(f => f.ClassNumber == number);
				context.Classrooms.Remove(obj);
				context.SaveChanges();
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.Message = ex.Message;
				logger.LogError(ex, ex.Message);
			}
			return response;
		}
	}
}
