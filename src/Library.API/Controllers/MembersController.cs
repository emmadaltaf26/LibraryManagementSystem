using Library.Application.DTOs;
using Library.Application.Features.Members.Commands;
using Library.Application.Features.Members.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all members
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll()
    {
        var members = await _mediator.Send(new GetAllMembersQuery());
        return Ok(members);
    }

    /// <summary>
    /// Get a member by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberDto>> GetById(Guid id)
    {
        var member = await _mediator.Send(new GetMemberByIdQuery(id));
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    /// <summary>
    /// Create a new member
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberDto dto)
    {
        var member = await _mediator.Send(new CreateMemberCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = member.Id }, member);
    }

    /// <summary>
    /// Update an existing member
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemberDto>> Update(Guid id, [FromBody] UpdateMemberDto dto)
    {
        var member = await _mediator.Send(new UpdateMemberCommand(id, dto));
        if (member == null)
            return NotFound();

        return Ok(member);
    }

    /// <summary>
    /// Delete a member
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMemberCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}
