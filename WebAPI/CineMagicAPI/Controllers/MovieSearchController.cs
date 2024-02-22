using Microsoft.AspNetCore.Mvc;
using CineMagicAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CineMagicAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieSearchController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        private const string TMDB_API_KEY = "30b4c1980f6c33f5036366f3c4def27b"; //tmdb'ye kayıt olunup key alındı.
        public MovieSearchController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<List<Movies>> GetMovieSearchAsync(string movieName)
        {

            List<Movies> movieSearch = new List<Movies>();
            string endpoint = $"https://api.themoviedb.org/3/search/movie?api_key={TMDB_API_KEY}&query={movieName}";
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<TMDbResponse3>(json);
                movieSearch = data?.results;
            }

            return movieSearch;
        }

        public class TMDbResponse3
        {
            public List<Movies> results { get; set; }
        }
    }
}
