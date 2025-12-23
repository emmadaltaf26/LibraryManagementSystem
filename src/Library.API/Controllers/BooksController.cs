using Library.Application.DTOs;
using Library.Application.Features.Books.Commands;
using Library.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all books
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
    {
        var books = await _mediator.Send(new GetAllBooksQuery());
        return Ok(books);
    }

    /// <summary>
    /// Get a book by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetById(Guid id)
    {
        var book = await _mediator.Send(new GetBookByIdQuery(id));
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    /// <summary>
    /// Create a new book
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
    {
        var book = await _mediator.Send(new CreateBookCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    /// <summary>
    /// Update an existing book
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookDto>> Update(Guid id, [FromBody] UpdateBookDto dto)
    {
        var book = await _mediator.Send(new UpdateBookCommand(id, dto));
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    /// <summary>
    /// Delete a book
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBookCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
