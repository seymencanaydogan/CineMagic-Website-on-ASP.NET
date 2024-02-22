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
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration)
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
        public bool register(Register register)
        {
            string password = register.HashPassword; // Kullanıcı tarafından sağlanan şifre
            string hashedPassword = HashPassword(password); // Şifreyi hashleme işlemi

            connectionString();
            com.Connection = con;
            com.CommandText = "INSERT INTO Users(Email,PasswordHash) VALUES(@Email, @PasswordHash)";
            com.Parameters.AddWithValue("@Email", register.Email);
            com.Parameters.AddWithValue("@PasswordHash",hashedPassword);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();

            if (i > 0)
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

    

