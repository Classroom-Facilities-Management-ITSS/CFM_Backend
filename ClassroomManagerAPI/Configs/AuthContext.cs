using System.Security.Claims;

namespace ClassroomManagerAPI.Configs
{
    public class AuthContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public string GetCurrentEmail()
        {
            try
            {
                return User?.FindFirst(ClaimTypes.Email)?.Value;
            } catch {
                return string.Empty;
            }
        }

        public Guid GetCurrentId()
        {
            try
            {
                return Guid.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public string GetCurrentRole()
        {
            try
            {
                return User?.FindFirst(ClaimTypes.Role)?.Value;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
