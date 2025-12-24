using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateAuthorCommand : IRequest<AuthorDto?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
