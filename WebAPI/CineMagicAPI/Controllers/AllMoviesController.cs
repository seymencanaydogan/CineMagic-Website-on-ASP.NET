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
    [Route("[controller]")]
    public class AllMoviesController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        private const string TMDB_API_KEY = "30b4c1980f6c33f5036366f3c4def27b"; //tmdb'ye kayıt olunup key alındı.

        public AllMoviesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<List<Movies>> GetAllMoviesAsync(int genre_ids)
        {
            List<Movies> allMovies = new List<Movies>();
            string endpoint = $"https://api.themoviedb.org/3/discover/movie?api_key={TMDB_API_KEY}&with_genres={genre_ids}";
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<TMDbResponse2>(json);
                allMovies = data?.results;
            }

            return allMovies;
        }
    }
        
        public class TMDbResponse2
        {
            public List<Movies> results { get; set; }
        }
    }


