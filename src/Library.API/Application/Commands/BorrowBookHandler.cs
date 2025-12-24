using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using MediatR;

namespace Library.API.Application.Commands;

public class BorrowBookHandler : IRequestHandler<BorrowBookCommand, LoanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BorrowBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.BookId, cancellationToken);
        if (book == null)
            throw new InvalidOperationException("Book not found");

        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("No copies available for borrowing");

        var member = await _unitOfWork.Members.GetByIdAsync(request.MemberId, cancellationToken);
        if (member == null)
            throw new InvalidOperationException("Member not found");

        if (!member.IsActive)
            throw new InvalidOperationException("Member account is not active");

        var existingLoan = await _unitOfWork.Loans.FindAsync(
            l => l.BookId == request.BookId &&
                 l.MemberId == request.MemberId &&
                 l.Status == LoanStatus.Active,
            cancellationToken);

        if (existingLoan.Any())
            throw new InvalidOperationException("Member already has this book on loan");

        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            BookId = request.BookId,
            MemberId = request.MemberId,
            BorrowDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(request.LoanDays),
            Status = LoanStatus.Active,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        book.AvailableCopies--;
        book.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Loans.AddAsync(loan, cancellationToken);
        await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdLoan = await _unitOfWork.Loans.GetByIdAsync(loan.Id, cancellationToken);
        return _mapper.Map<LoanDto>(createdLoan);
    }
}
