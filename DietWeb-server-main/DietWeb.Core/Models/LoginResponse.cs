namespace DietWeb.API.DTOs
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; } // Optional JWT or session token
    }

}
