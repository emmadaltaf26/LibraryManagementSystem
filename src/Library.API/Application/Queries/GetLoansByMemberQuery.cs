using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetLoansByMemberQuery : IRequest<IEnumerable<LoanDto>>
{
    public Guid MemberId { get; set; }
}
