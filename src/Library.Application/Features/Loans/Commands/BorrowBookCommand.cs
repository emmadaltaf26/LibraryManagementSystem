using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Features.Loans.Commands;

public class BorrowBookCommand : IRequest<LoanDto>
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public int LoanDays { get; set; } = 14;
    public string? Notes { get; set; }
}

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required");

        RuleFor(x => x.LoanDays)
            .InclusiveBetween(1, 90).WithMessage("Loan period must be between 1 and 90 days");
    }
}

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, LoanDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BorrowBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
