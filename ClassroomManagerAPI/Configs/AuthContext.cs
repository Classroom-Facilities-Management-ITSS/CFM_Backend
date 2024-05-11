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
            return User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetCurrentId()
        {
            return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetCurrentRole()
        {
            return User?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
