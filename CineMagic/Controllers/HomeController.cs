using CineMagic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CineMagicAPI.Controllers;
using CineMagicAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Data.SqlClient;


namespace CineMagic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly LoginController _loginController;

        private readonly RegisterController _registerController;


        public HomeController(ILogger<HomeController> logger, LoginController loginController, RegisterController registerController)
        {
            _logger = logger;
            _loginController = loginController;
            _registerController = registerController;
        }

        public async Task<IActionResult> Index()
        {
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var popularMoviesService = scope.ServiceProvider.GetRequiredService<PopularMoviesController>();
                List<Movies> popularMovies = await popularMoviesService.GetPopularMoviesAsync();
                return View(popularMovies);
            }
        }
        public async Task<IActionResult> MovieDetail(int id)
        {
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var movieDetailService = scope.ServiceProvider.GetRequiredService<MovieDetailController>();
                var movie = await movieDetailService.GetMovieDetailAsync(id);
                return View(movie);
            }

        }

        public async Task<IActionResult> FilmAra(string title)
        {
                using (var scope = HttpContext.RequestServices.CreateScope())
                {
                    var movieSearchService = scope.ServiceProvider.GetRequiredService<MovieSearchController>();
                    List<Movies> searchMovies = await movieSearchService.GetMovieSearchAsync(title);
                    return View(searchMovies);
                }
        }

        public IActionResult Filmler()
        {
            return View();
        }

        public async Task<IActionResult> KategoriAra(int genre_ids)
         {
                using (var scope = HttpContext.RequestServices.CreateScope())
                {
                    var allMoviesService = scope.ServiceProvider.GetRequiredService<AllMoviesController>();
                    List<Movies> allMovies = await allMoviesService.GetAllMoviesAsync(genre_ids);
                    return View(allMovies);
                } 

        } 

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                bool loginResult = _loginController.login(login);

                if (loginResult)
                {
                    List<Claim> claims= new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, login.Email)
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = login.KeepLoggedIn

                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "E-posta veya þifre yanlýþ! Lütfen yeniden deneyiniz.";
                    return View(login);
                }
            }
            else
            {
                return View(login);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            try
            {
                bool registerResult = _registerController.register(register);

                if (registerResult)
                {
                    ViewBag.SuccessMessage= "Kullanýcý kaydý baþarýyla tamamlandý.";
                    return View(register);
                }
                else
                {
                    ViewBag.ErrorMessage = "Kullanýcý kaydý yapýlamadý! Lütfen bilgilerinizi kontrol edip tekrar deneyiniz.";
                    return View(register);
                }
            }
            catch (SqlException ex) when (ex.Message.Contains("UNIQUE KEY constraint"))
            {
                    ViewBag.ErrorMessage = "Bu e-posta adresi zaten kayýtlý.";
                    return View();
            }
        }
 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Hakkinda()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
    
