using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Books.Commands;

public record UpdateBookCommand(Guid Id, UpdateBookDto Book) : IRequest<BookDto?>;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Book ID is required");

        RuleFor(x => x.Book.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Book.ISBN)
            .NotEmpty().WithMessage("ISBN is required")
            .MaximumLength(20).WithMessage("ISBN cannot exceed 20 characters");

        RuleFor(x => x.Book.PublishedYear)
            .InclusiveBetween(1000, DateTime.Now.Year + 1)
            .WithMessage($"Published year must be between 1000 and {DateTime.Now.Year + 1}");

        RuleFor(x => x.Book.TotalCopies)
            .GreaterThan(0).WithMessage("Total copies must be greater than 0");
    }
}

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
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

        book.Title = request.Book.Title;
        book.ISBN = request.Book.ISBN;
        book.Description = request.Book.Description;
        book.PublishedYear = request.Book.PublishedYear;
        book.TotalCopies = request.Book.TotalCopies;
        book.AuthorId = request.Book.AuthorId;
        book.GenreId = request.Book.GenreId;
        book.UpdatedAt = DateTime.UtcNow;

        // Recalculate available copies
        book.AvailableCopies = Math.Max(0, request.Book.TotalCopies - borrowedCopies);

        await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedBook = await _unitOfWork.Books.GetByIdAsync(book.Id, cancellationToken);
        return _mapper.Map<BookDto>(updatedBook);
    }
}
