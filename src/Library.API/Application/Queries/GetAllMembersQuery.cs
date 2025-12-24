using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAllMembersQuery : IRequest<IEnumerable<MemberDto>>
{
}
