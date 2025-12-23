using Library.Application.Common.Interfaces;
using MediatR;

namespace Library.Application.Features.Genres.Commands;

public record DeleteGenreCommand(Guid Id) : IRequest<bool>;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);
        if (genre == null)
            return false;

        // Check if genre has books
        var books = await _unitOfWork.Books.FindAsync(b => b.GenreId == request.Id, cancellationToken);
        if (books.Any())
            throw new InvalidOperationException("Cannot delete a genre with existing books");

        await _unitOfWork.Genres.DeleteAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
