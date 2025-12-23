using Library.Application.DTOs;
using Library.Application.Features.Genres.Commands;
using Library.Application.Features.Genres.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll()
    {
        var genres = await _mediator.Send(new GetAllGenresQuery());
        return Ok(genres);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GenreDto>> GetById(Guid id)
    {
        var genre = await _mediator.Send(new GetGenreByIdQuery(id));
        if (genre == null)
            return NotFound();

        return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create([FromBody] CreateGenreDto dto)
    {
        var genre = await _mediator.Send(new CreateGenreCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<GenreDto>> Update(Guid id, [FromBody] UpdateGenreDto dto)
    {
        var genre = await _mediator.Send(new UpdateGenreCommand(id, dto));
        if (genre == null)
            return NotFound();

        return Ok(genre);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteGenreCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
