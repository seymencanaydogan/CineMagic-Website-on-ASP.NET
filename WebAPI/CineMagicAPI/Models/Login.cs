namespace CineMagicAPI.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public bool KeepLoggedIn { get; set; }=true;
    }
}
