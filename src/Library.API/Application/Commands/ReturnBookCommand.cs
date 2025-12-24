using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class ReturnBookCommand : IRequest<LoanDto?>
{
    public Guid LoanId { get; set; }
    public string? Notes { get; set; }
}
