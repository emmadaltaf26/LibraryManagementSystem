using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Features.Loans.Commands;

public record ReturnBookCommand(Guid LoanId, ReturnBookDto? ReturnInfo = null) : IRequest<LoanDto?>;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, LoanDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReturnBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanDto?> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(request.LoanId, cancellationToken);
        if (loan == null)
            return null;

        if (loan.Status == LoanStatus.Returned)
            throw new InvalidOperationException("This book has already been returned");

        // Update loan
        loan.ReturnDate = DateTime.UtcNow;
        loan.Status = LoanStatus.Returned;
        loan.UpdatedAt = DateTime.UtcNow;

        if (request.ReturnInfo?.Notes != null)
            loan.Notes = string.IsNullOrEmpty(loan.Notes)
                ? request.ReturnInfo.Notes
                : $"{loan.Notes}; {request.ReturnInfo.Notes}";

        // Update book availability
        var book = await _unitOfWork.Books.GetByIdAsync(loan.BookId, cancellationToken);
        if (book != null)
        {
            book.AvailableCopies++;
            book.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
        }

        await _unitOfWork.Loans.UpdateAsync(loan, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedLoan = await _unitOfWork.Loans.GetByIdAsync(loan.Id, cancellationToken);
        return _mapper.Map<LoanDto>(updatedLoan);
    }
}
