using CFM.Web.Models;

namespace CFM.Web.Service.IService
{
	public interface IFacilityService
	{
		Task<ResponseDto?> GetFacilityAsync(string name);
		Task<ResponseDto?> GetAllFacilityAsync();
		Task<ResponseDto?> GetFacilityByIdAsync(int id);
		Task<ResponseDto?> CreateFacilityAsync(FacilityDto facilityDto);
		Task<ResponseDto?> UpdateFacilityAsync(FacilityDto facilityDto);
		Task<ResponseDto?> DeleteFacilityAsync(int id);
	}
}
