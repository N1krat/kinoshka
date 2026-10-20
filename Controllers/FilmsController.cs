using System.Text.Json;
using kino.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly string _path;
    private static readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

    public FilmsController(IWebHostEnvironment env)
        => _path = Path.Combine(env.ContentRootPath, "data", "films.json");

    private async Task<List<Film>> ReadFilms()
    {
        var json = await System.IO.File.ReadAllTextAsync(_path);
        return JsonSerializer.Deserialize<List<Film>>(json, _opts) ?? new();
    }

    [HttpGet]
    public async Task<ActionResult<List<Film>>> GetAll()
        => await ReadFilms();

    [HttpGet("{id}")]
    public async Task<ActionResult<Film>> GetById(int id)
    {
        var film = (await ReadFilms()).FirstOrDefault(f => f.Id == id);
        return film is null ? NotFound() : film;
    }
}