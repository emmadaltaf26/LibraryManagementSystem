using MediatR;

namespace Library.API.Application.Commands;

public class DeleteAuthorCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
