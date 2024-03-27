using CFM.Web.Models;
using CFM.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static CFM.Web.Models.Utility.SD;

namespace CFM.Web.Service
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpFactory;

		public BaseService(IHttpClientFactory httpFactory)
		{
			_httpFactory = httpFactory;
		}
		public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
		{
			try
			{
				HttpClient client = _httpFactory.CreateClient("CFMApi");
				HttpRequestMessage message = new();
				message.Headers.Add("Accept", "application/json");

				//token
				message.RequestUri = new Uri(requestDto.Url);
				if (requestDto.Data != null)
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
				}

				HttpResponseMessage? apiResponse = null;

				switch (requestDto.ApiType)
				{
					case ApiType.POST:
						message.Method = HttpMethod.Post;
						break;
					case ApiType.DELETE:
						message.Method = HttpMethod.Delete;
						break;
					case ApiType.PUT:
						message.Method = HttpMethod.Put;
						break;
					default:
						message.Method = HttpMethod.Get;
						break;
				}

				apiResponse = await client.SendAsync(message);

				switch (apiResponse.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new() { IsSuccess = false, Message = "Not found" };
						break;
					case HttpStatusCode.Forbidden:
						return new() { IsSuccess = false, Message = "Access denied" };
						break;
					case HttpStatusCode.Unauthorized:
						return new() { IsSuccess = false, Message = "Unauthorized" };
						break;
					case HttpStatusCode.InternalServerError:
						return new() { IsSuccess = false, Message = "Internal Server error" };
						break;
					default:
						var apiContent = await apiResponse.Content.ReadAsStringAsync();
						var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
						return apiResponseDto;
				}
			}
			catch (Exception ex)
			{
				var dto = new ResponseDto()
				{
					Message = ex.Message.ToString(),
					IsSuccess = false
				};
				return dto;
			}
		}	
	}
}
