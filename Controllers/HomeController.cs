using System.Diagnostics;
using System.Text.Json;
using kino.Models;
using Microsoft.AspNetCore.Mvc;

namespace kino.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;
    public HomeController(IWebHostEnvironment env) => _env = env;

    public IActionResult Index() => View();
    public IActionResult Theaters() => View();
    public IActionResult Program() => View();
    public IActionResult Contact() => View();
    public IActionResult Shop() => View();
    public IActionResult News() => View();
    public IActionResult Registration() => View();

    public async Task<IActionResult> Movies()
    {
        var path = Path.Combine(_env.ContentRootPath, "data", "films.json");
        var json = await System.IO.File.ReadAllTextAsync(path);
        var films = JsonSerializer.Deserialize<List<Film>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        return View(films);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}