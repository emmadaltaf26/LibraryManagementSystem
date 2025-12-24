using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetOverdueLoansQuery : IRequest<IEnumerable<LoanDto>>
{
}
