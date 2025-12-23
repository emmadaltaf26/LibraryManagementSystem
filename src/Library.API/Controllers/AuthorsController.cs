using Library.Application.DTOs;
using Library.Application.Features.Authors.Commands;
using Library.Application.Features.Authors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all authors
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        var authors = await _mediator.Send(new GetAllAuthorsQuery());
        return Ok(authors);
    }

    /// <summary>
    /// Get an author by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuthorDto>> GetById(Guid id)
    {
        var author = await _mediator.Send(new GetAuthorByIdQuery(id));
        if (author == null)
            return NotFound();

        return Ok(author);
    }

    /// <summary>
    /// Create a new author
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto dto)
    {
        var author = await _mediator.Send(new CreateAuthorCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    /// <summary>
    /// Update an existing author
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AuthorDto>> Update(Guid id, [FromBody] UpdateAuthorDto dto)
    {
        var author = await _mediator.Send(new UpdateAuthorCommand(id, dto));
        if (author == null)
            return NotFound();

        return Ok(author);
    }

    /// <summary>
    /// Delete an author
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteAuthorCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
