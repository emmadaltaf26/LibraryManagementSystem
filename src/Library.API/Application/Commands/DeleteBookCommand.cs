using MediatR;

namespace Library.API.Application.Commands;

public class DeleteBookCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
