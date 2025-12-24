using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetLoanByIdQuery : IRequest<LoanDto?>
{
    public Guid Id { get; set; }
}
