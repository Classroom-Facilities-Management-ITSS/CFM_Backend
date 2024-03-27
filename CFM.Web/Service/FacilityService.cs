using CFM.Web.Models;
using CFM.Web.Models.Utility;
using CFM.Web.Service.IService;

namespace CFM.Web.Service
{
	public class FacilityService : IFacilityService
	{
		private readonly IBaseService _baseService;

		public FacilityService(IBaseService baseService)
        {
			_baseService = baseService;
		}
        public async Task<ResponseDto?> CreateFacilityAsync(FacilityDto facilityDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = facilityDto,
				Url = SD.FacilityAPIBase + "api/facilities"
			});
		}

		public async Task<ResponseDto?> DeleteFacilityAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.DELETE,
				Url = SD.FacilityAPIBase + "api/facilities/" + id 
			});
		}

		public async Task<ResponseDto?> GetAllFacilityAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.FacilityAPIBase + "api/facilities"
			});
		}

		public async Task<ResponseDto?> GetFacilityAsync(string name)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.FacilityAPIBase + "api/faciliies/GetByName/" + name
			});
		}

		public async Task<ResponseDto?> GetFacilityByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.FacilityAPIBase + "api/facilities/" + id
			});
		}

		public async Task<ResponseDto?> UpdateFacilityAsync(FacilityDto facilityDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.POST,
				Data = facilityDto,
				Url = SD.FacilityAPIBase + "api/facilities"
			});
		}
	}
}
