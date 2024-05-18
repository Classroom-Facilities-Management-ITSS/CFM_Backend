namespace ClassroomManagerAPI.Models.Auth
{
    public class AuthModel
    {
        public string? AccessToken { get; set; }
        public string? Email { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Role { get; set; }
	}
}
