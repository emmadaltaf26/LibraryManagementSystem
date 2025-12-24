using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAllGenresQuery : IRequest<IEnumerable<GenreDto>>
{
}
