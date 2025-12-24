using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAllLoansQuery : IRequest<IEnumerable<LoanDto>>
{
}
