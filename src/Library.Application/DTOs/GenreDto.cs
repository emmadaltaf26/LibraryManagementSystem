namespace Library.Application.DTOs;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGenreDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateGenreDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
