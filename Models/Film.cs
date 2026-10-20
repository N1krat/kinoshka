namespace kino.Models;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Genre { get; set; } = "";
    public int Duration { get; set; }
    public int Year { get; set; }
    public string Poster { get; set; } = "";
}