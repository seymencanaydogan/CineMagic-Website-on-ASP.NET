using Microsoft.AspNetCore.Mvc;
using CineMagicAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace CineMagicAPI.Controllers
{
    [ApiController]
    [Route("Home/[controller]")]
    public class MovieDetailController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        private const string TMDB_API_KEY = "30b4c1980f6c33f5036366f3c4def27b";

        public MovieDetailController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<MovieDetail> GetMovieDetailAsync(int movieID)
        {
            MovieDetail movieDetail = null;
            string endpoint = $"https://api.themoviedb.org/3/movie/{movieID}?api_key={TMDB_API_KEY}";
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                movieDetail = JsonSerializer.Deserialize<MovieDetail>(json);
            }

            return movieDetail;
        }
    }

}

