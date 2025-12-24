using MediatR;

namespace Library.API.Application.Commands;

public class DeleteMemberCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
