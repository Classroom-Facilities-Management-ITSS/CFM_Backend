namespace ClassroomManagerAPI.Services.IServices
{
    public interface IBCryptService
    {
        public string HashPassword(string password);
        public bool verifyPassword(string password, string hashedPassword);

    }
}
