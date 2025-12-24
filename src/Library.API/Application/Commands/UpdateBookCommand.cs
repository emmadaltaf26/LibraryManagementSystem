using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateBookCommand : IRequest<BookDto?>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }
}
