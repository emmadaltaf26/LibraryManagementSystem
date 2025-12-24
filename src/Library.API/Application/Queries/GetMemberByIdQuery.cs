using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetMemberByIdQuery : IRequest<MemberDto?>
{
    public Guid Id { get; set; }
}
