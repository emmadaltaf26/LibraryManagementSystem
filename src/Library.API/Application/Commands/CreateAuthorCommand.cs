using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
