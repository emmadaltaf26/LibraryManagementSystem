using Library.API.Application.Commands;
using Library.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetAllLoansQuery());
        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetLoanByIdQuery { Id = id });
        if (response == null)
            return NotFound(new { Message = "Loan not found." });

        return Ok(response);
    }

    [HttpGet]
    [Route("member/{memberId}")]
    public async Task<IActionResult> GetByMember([FromRoute] Guid memberId)
    {
        var response = await _mediator.Send(new GetLoansByMemberQuery { MemberId = memberId });
        return Ok(response);
    }

    [HttpGet]
    [Route("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        var response = await _mediator.Send(new GetOverdueLoansQuery());
        return Ok(response);
    }

    [HttpPost]
    [Route("borrow")]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost]
    [Route("{id}/return")]
    public async Task<IActionResult> ReturnBook([FromRoute] Guid id, [FromBody] ReturnBookCommand command)
    {
        command.LoanId = id;
        var response = await _mediator.Send(command);
        if (response == null)
            return NotFound(new { Message = "Loan not found." });

        return Ok(response);
    }
}
