using AutoMapper;
using ClassroomManagerAPI.Configs;
using ClassroomManagerAPI.Configs.Infastructure;
using ClassroomManagerAPI.Entities;
using ClassroomManagerAPI.Models.Dto;
using ClassroomManagerAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClassroomManagerAPI.Controllers
{
    [ApiVersion(Settings.APIVersion)]
    [Route(Settings.APIDefaultRoute + "/facility")]
    [ApiController]
	public class FacilityAPIController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IFacilityRepository _facilityRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<FacilityAPIController> _logger;

		public FacilityAPIController(AppDbContext context, IMapper mapper, ILogger<FacilityAPIController> logger, IFacilityRepository facilityRepository)
		{
			_context = context;
			_facilityRepository = facilityRepository;
			_mapper = mapper;
			_logger = logger;
		}

		// Get all
		[HttpGet, Route("facilities")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				//_logger.LogInformation("GetAll was invoked");
				var facilitiesDomain = await _facilityRepository.GetAllAsync();
				var facilitiesDto = _mapper.Map<List<FacilityDto>>(facilitiesDomain);

				_logger.LogInformation($"Finished get all request with data: {JsonSerializer.Serialize(facilitiesDomain)}");
				return Ok(facilitiesDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Get by id
		[HttpGet]
		[Route("/facilities/{id:Guid}")]
		public async Task<IActionResult> GetById([FromRoute]Guid id)
		{
			try
			{
				var facilityDomain = await _facilityRepository.GetByIdAsync(id);

				if (facilityDomain == null)
				{
					return NotFound();
				}

				return Ok(_mapper.Map<FacilityDto>(facilityDomain));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Get by name
		[HttpGet]
		[Route("facilities/GetByName/{name}")]
		public async Task<IActionResult> GetByName(string name)
		{
			try
			{
				var facilityDomain = await _facilityRepository.GetByNameAsync(name);
				if (facilityDomain == null)
				{
					return NotFound();
				}
				return Ok(_mapper.Map<FacilityDto>(facilityDomain));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}


		// Create facility
		[HttpPost, Route("facilities/Add")]
		public async Task<IActionResult> Create([FromBody] AddFacilityRequestDto addFacilityRequestDto)
		{
			try
			{
				var facilityDomain = _mapper.Map<Facility>(addFacilityRequestDto);
				facilityDomain = await _facilityRepository.CreateAsync(facilityDomain);
				var facilityDto = _mapper.Map<FacilityDto>(facilityDomain);
				return CreatedAtAction(nameof(GetById), new { id = facilityDomain.Id }, facilityDto);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Update facility
		[HttpPut, Route("{id:Guid}")]
		public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateFacilityRequestDto updateFacilityRequestDto)
		{
			try
			{
				// Map Dto to domain models
				var facilityDomain = _mapper.Map<Facility>(updateFacilityRequestDto);
				
				// check if exists
				facilityDomain = await _facilityRepository.UpdateAsync(id, facilityDomain);

				if (facilityDomain == null)
				{
					return NotFound();
				}

				await _context.SaveChangesAsync();
				return Ok(_mapper.Map<Facility>(facilityDomain));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}

		// Remove a facility
		[HttpDelete]
		[Route("facilities/{id:Guid}")]
		public async Task<IActionResult> Delete([FromRoute]Guid id)
		{
			try
			{
				var facilityDomain = await _facilityRepository.DeleteAsync(id);
				if (facilityDomain == null)
				{
					return NotFound();
				}

				return Ok(_mapper.Map<FacilityDto>(facilityDomain));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				throw;
			}
		}
	}
}
