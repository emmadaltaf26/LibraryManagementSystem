using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id, cancellationToken);
        if (book == null)
            return null;

        var previousTotalCopies = book.TotalCopies;
        var borrowedCopies = previousTotalCopies - book.AvailableCopies;

        book.Title = request.Title;
        book.ISBN = request.ISBN;
        book.Description = request.Description;
        book.PublishedYear = request.PublishedYear;
        book.TotalCopies = request.TotalCopies;
        book.AuthorId = request.AuthorId;
        book.GenreId = request.GenreId;
        book.UpdatedAt = DateTime.UtcNow;
        book.AvailableCopies = Math.Max(0, request.TotalCopies - borrowedCopies);

        await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedBook = await _unitOfWork.Books.GetByIdAsync(book.Id, cancellationToken);
        return _mapper.Map<BookDto>(updatedBook);
    }
}
