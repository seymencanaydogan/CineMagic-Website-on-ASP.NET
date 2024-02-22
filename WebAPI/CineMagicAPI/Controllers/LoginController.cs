using CineMagicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace CineMagicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        void connectionString()
        {
            con.ConnectionString = "data source=DESKTOP-TRRS3IN\\SQLEXPRESS; database=CineMagic; integrated security=SSPI; TrustServerCertificate=true;";
        }

        Response res = new Response();

        [HttpPost]
        public bool login(Login login)
        {
            string password = login.HashPassword; // Kullanıcı tarafından girilen şifre
            string hashedPassword = HashPassword(password); // Şifreyi hashleme işlemi
            connectionString();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Users WHERE Email=@Email AND PasswordHash=@PasswordHash";
            com.Parameters.AddWithValue("@Email", login.Email);
            com.Parameters.AddWithValue("@PasswordHash", hashedPassword);

            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                res.status = true;
                return res.status;
            }
            else
            {
                res.status = false;
                return res.status;
            }
        }
    }
}
