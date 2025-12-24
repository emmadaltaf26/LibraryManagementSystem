using MediatR;

namespace Library.API.Application.Commands;

public class DeleteGenreCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
