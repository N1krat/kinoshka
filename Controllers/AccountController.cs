using System.Security.Claims;
using System.Text.Json;
using kino.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace kino.Controllers;

public class AccountController : Controller
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public AccountController(IWebHostEnvironment env)
        => _path = Path.Combine(env.ContentRootPath, "data", "users.json");

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var json = await System.IO.File.ReadAllTextAsync(_path);
        var users = JsonSerializer.Deserialize<List<User>>(json, _opts) ?? new();

        var user = users.FirstOrDefault(u =>
            u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) &&
            u.Password == model.Password);

        if (user is null)
        {
            ViewBag.Error = "Email sau parolă greșită.";
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return user.Role == "Admin"
            ? RedirectToAction("Index", "Admin")
            : RedirectToAction("Index", "User");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}