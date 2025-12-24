using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateGenreCommand : IRequest<GenreDto?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
