using Library.Application.Common.Interfaces;
using Library.Domain.Enums;
using MediatR;

namespace Library.API.Application.Commands;

public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id, cancellationToken);
        if (book == null)
            return false;

        var activeLoans = await _unitOfWork.Loans.FindAsync(
            l => l.BookId == request.Id && l.Status == LoanStatus.Active,
            cancellationToken);

        if (activeLoans.Any())
            throw new InvalidOperationException("Cannot delete a book with active loans");

        await _unitOfWork.Books.DeleteAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
