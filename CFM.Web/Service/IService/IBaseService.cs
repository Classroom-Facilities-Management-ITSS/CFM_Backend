using CFM.Web.Models;

namespace CFM.Web.Service.IService
{
	public interface IBaseService
	{
		Task<ResponseDto?> SendAsync(RequestDto requestDto);
	}
}
