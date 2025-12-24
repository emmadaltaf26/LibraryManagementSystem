namespace Library.Application.DTOs;

public class AuthorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int BookCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class UpdateAuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
