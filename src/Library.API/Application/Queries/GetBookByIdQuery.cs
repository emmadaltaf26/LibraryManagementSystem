using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public Guid Id { get; set; }
}
