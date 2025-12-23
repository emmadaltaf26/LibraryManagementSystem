using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Features.Books.Commands;

public record CreateBookCommand(CreateBookDto Book) : IRequest<BookDto>;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
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

        RuleFor(x => x.Book.AuthorId)
            .NotEmpty().WithMessage("Author is required");

        RuleFor(x => x.Book.GenreId)
            .NotEmpty().WithMessage("Genre is required");
    }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = _mapper.Map<Book>(request.Book);
        book.Id = Guid.NewGuid();
        book.CreatedAt = DateTime.UtcNow;
        book.AvailableCopies = book.TotalCopies;

        await _unitOfWork.Books.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload with navigation properties
        var createdBook = await _unitOfWork.Books.GetByIdAsync(book.Id, cancellationToken);
        return _mapper.Map<BookDto>(createdBook);
    }
}
