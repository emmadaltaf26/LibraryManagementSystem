using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Features.Books.Commands;

public class CreateBookCommand : IRequest<BookDto>
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }
}

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.ISBN)
            .NotEmpty().WithMessage("ISBN is required")
            .MaximumLength(20).WithMessage("ISBN cannot exceed 20 characters");

        RuleFor(x => x.PublishedYear)
            .InclusiveBetween(1000, DateTime.Now.Year + 1)
            .WithMessage($"Published year must be between 1000 and {DateTime.Now.Year + 1}");

        RuleFor(x => x.TotalCopies)
            .GreaterThan(0).WithMessage("Total copies must be greater than 0");

        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author is required");

        RuleFor(x => x.GenreId)
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
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            ISBN = request.ISBN,
            Description = request.Description,
            PublishedYear = request.PublishedYear,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.TotalCopies,
            AuthorId = request.AuthorId,
            GenreId = request.GenreId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Books.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdBook = await _unitOfWork.Books.GetByIdAsync(book.Id, cancellationToken);
        return _mapper.Map<BookDto>(createdBook);
    }
}
