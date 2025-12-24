using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetGenreByIdQuery : IRequest<GenreDto?>
{
    public Guid Id { get; set; }
}
