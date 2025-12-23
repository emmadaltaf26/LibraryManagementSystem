using Library.Application.Features.Members.Commands;
using Library.Application.Features.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetAllMembersQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetMemberByIdQuery { Id = id });
        if (response == null)
            return NotFound(new { Message = "Member not found." });

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMemberCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateMemberCommand command)
    {
        var response = await _mediator.Send(command);
        if (response == null)
            return NotFound(new { Message = "Member not found." });

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteMemberCommand { Id = id });
        if (!result)
            return NotFound(new { Message = "Member not found." });

        return Ok(new { Message = "Member deleted successfully." });
    }
}
