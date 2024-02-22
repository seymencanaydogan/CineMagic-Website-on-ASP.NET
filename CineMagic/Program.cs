using CineMagicAPI.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// !!!
builder.Services.AddScoped<PopularMoviesController>();
builder.Services.AddScoped<AllMoviesController>();
builder.Services.AddScoped<MovieDetailController>();
builder.Services.AddScoped<MovieSearchController>();
builder.Services.AddScoped<RegisterController>();
builder.Services.AddScoped<LoginController>();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option=> { option.LoginPath = "/Home/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
