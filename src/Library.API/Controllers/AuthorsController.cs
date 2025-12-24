using Library.API.Application.Commands;
using Library.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetAllAuthorsQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetAuthorByIdQuery { Id = id });
        if (response == null)
            return NotFound(new { Message = "Author not found." });

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuthorCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAuthorCommand command)
    {
        var response = await _mediator.Send(command);
        if (response == null)
            return NotFound(new { Message = "Author not found." });

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteAuthorCommand { Id = id });
        if (!result)
            return NotFound(new { Message = "Author not found." });

        return Ok(new { Message = "Author deleted successfully." });
    }
}
