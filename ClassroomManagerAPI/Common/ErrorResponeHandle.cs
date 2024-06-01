using ClassroomManagerAPI.Enums;

namespace ClassroomManagerAPI.Common
{
    public class ErrorResponeHandle
    {
        private readonly RequestDelegate _next;
        public ErrorResponeHandle(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var responese = new BadResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                responese.AddBadRequest(nameof(ErrorSystemEnum.ServerError));
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(responese);
            }
        }
    }
}
