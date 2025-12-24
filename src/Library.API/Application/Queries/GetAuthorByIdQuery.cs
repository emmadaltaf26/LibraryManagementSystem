using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAuthorByIdQuery : IRequest<AuthorDto?>
{
    public Guid Id { get; set; }
}
