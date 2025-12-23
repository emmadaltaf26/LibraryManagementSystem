using Library.Application.Common.Interfaces;
using MediatR;

namespace Library.Application.Features.Authors.Commands;

public class DeleteAuthorCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(request.Id, cancellationToken);
        if (author == null)
            return false;

        var books = await _unitOfWork.Books.FindAsync(b => b.AuthorId == request.Id, cancellationToken);
        if (books.Any())
            throw new InvalidOperationException("Cannot delete an author with existing books");

        await _unitOfWork.Authors.DeleteAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
