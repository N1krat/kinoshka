using System.Security.Claims;
using System.Text.Json;
using kino.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public AuthController(IWebHostEnvironment env)
        => _path = Path.Combine(env.ContentRootPath, "data", "users.json");

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var json = await System.IO.File.ReadAllTextAsync(_path);
        var users = JsonSerializer.Deserialize<List<User>>(json, _opts) ?? new();

        var user = users.FirstOrDefault(u =>
            u.Email.Equals(req.Email, StringComparison.OrdinalIgnoreCase) &&
            u.Password == req.Password);

        if (user is null)
            return Unauthorized(new { message = "Email sau parolă greșită." });

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Ok(new { redirect = user.Role == "Admin" ? "/Admin" : "/User" });
    }

    // POST /api/auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}