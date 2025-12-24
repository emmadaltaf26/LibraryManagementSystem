using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.API.Application.Commands;

public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBookHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
