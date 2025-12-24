using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class CreateGenreCommand : IRequest<GenreDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
