using Library.API.Application.Commands;
using Library.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetAllBooksQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetBookByIdQuery { Id = id });
        if (response == null)
            return NotFound(new { Message = "Book not found." });

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBookCommand command)
    {
        var response = await _mediator.Send(command);
        if (response == null)
            return NotFound(new { Message = "Book not found." });

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteBookCommand { Id = id });
        if (!result)
            return NotFound(new { Message = "Book not found." });

        return Ok(new { Message = "Book deleted successfully." });
    }
}
