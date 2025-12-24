using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class BorrowBookCommand : IRequest<LoanDto>
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public int LoanDays { get; set; } = 14;
    public string? Notes { get; set; }
}
