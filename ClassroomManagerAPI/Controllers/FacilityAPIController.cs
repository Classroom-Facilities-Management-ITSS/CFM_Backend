using AutoMapper;
using ClassroomManagerAPI.Data;
using ClassroomManagerAPI.Models;
using ClassroomManagerAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagerAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FacilityAPIController : ControllerBase
	{
		private readonly AppDbContext _context;
		private ResponseDto _response;
		private IMapper _mapper;

		public FacilityAPIController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_response = new ResponseDto();
			_mapper = mapper;
		}

		[HttpGet]
		public ResponseDto Get()
		{
			try
			{
				IEnumerable<Facility> objList = _context.Facilities.ToList();
				_response.Result = _mapper.Map<IEnumerable<FacilityDto>>(objList);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}
			return _response;
		}

		[HttpGet]
		[Route("{id}")]
		public ResponseDto Get(int id)
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
			}
			return _response;
		}
	}
}
