using AutoMapper;
using ClassroomManagerAPI.Data;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClassroomManagerAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FacilityAPIController : ControllerBase
	{
		private readonly AppDbContext _context;
		private ResponseDto _response;
		private readonly IMapper _mapper;
		private readonly ILogger<FacilityAPIController> _logger;

		public FacilityAPIController(AppDbContext context, IMapper mapper, ILogger<FacilityAPIController> logger)
		{
			_context = context;
			_response = new ResponseDto();
			_mapper = mapper;
			_logger = logger;
		}

		// Get all
		[HttpGet, Route("facilities")]
		public ResponseDto GetAll()
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				IEnumerable<Facility> objList = _context.Facilities.ToList();
				_response.Result = _mapper.Map<IEnumerable<FacilityDto>>(objList);
				//_logger.LogInformation($"Finished get all request with data: {JsonSerializer.Serialize(objList)}");
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
				throw;
			}
			return _response;
		}

		// Get by id
		[HttpGet]
		[Route("/facilities/{id:Guid}")]
		public ResponseDto GetById([FromRoute]Guid id)
		{
			try
			{
				Facility obj = _context.Facilities.First(u => u.Id == id);
				_response.Result = _mapper.Map<FacilityDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
				throw;
			}
			return _response;
		}

		// Get by name
		[HttpGet]
		[Route("facilities/GetByName/{name}")]
		public ResponseDto GetByName(string name)
		{
			try
			{
				Facility obj = _context.Facilities.FirstOrDefault(u => u.Name.ToLower() == name.ToLower());
				if (obj == null)
				{
					_response.IsSuccess = false;
				}
				_response.Result = _mapper.Map<FacilityDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
				throw;
			}
			return _response;
		}


		// Create facility
		[HttpPost]
		public ResponseDto Create([FromBody] AddFacilityRequestDto addFacilityRequestDto)
		{
			try
			{
				Facility obj = _mapper.Map<Facility>(addFacilityRequestDto);
				_context.Facilities.Add(obj);
				_context.SaveChanges();
				_response.Result = _mapper.Map<FacilityDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
			}
			return _response;
		}

		// Update facility
		[HttpPut]
		public ResponseDto Update([FromBody] FacilityDto facilityDto)
		{
			try
			{
				Facility obj = _mapper.Map<Facility>(facilityDto);
				_context.Facilities.Update(obj);
				_context.SaveChanges();
				_response.Result = _mapper.Map<FacilityDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
			}
			return _response;
		}

		// Remove a facility
		[HttpDelete]
		[Route("facilities/{id:Guid}")]
		public ResponseDto Delete([FromRoute]Guid id)
		{
			try
			{
				Facility obj = _context.Facilities.First(f => f.Id == id);
				_context.Facilities.Remove(obj);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
				_logger.LogError(ex, ex.Message);
			}
			return _response;
		}
	}
}
