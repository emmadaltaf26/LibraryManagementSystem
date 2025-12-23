using Library.Application.DTOs;
using Library.Application.Features.Loans.Commands;
using Library.Application.Features.Loans.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll()
    {
        var loans = await _mediator.Send(new GetAllLoansQuery());
        return Ok(loans);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LoanDto>> GetById(Guid id)
    {
        var loan = await _mediator.Send(new GetLoanByIdQuery(id));
        if (loan == null)
            return NotFound();

        return Ok(loan);
    }

    [HttpGet("member/{memberId:guid}")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetByMember(Guid memberId)
    {
        var loans = await _mediator.Send(new GetLoansByMemberQuery(memberId));
        return Ok(loans);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<LoanDto>>> GetOverdue()
    {
        var loans = await _mediator.Send(new GetOverdueLoansQuery());
        return Ok(loans);
    }

    [HttpPost("borrow")]
    public async Task<ActionResult<LoanDto>> BorrowBook([FromBody] BorrowBookDto dto)
    {
        var loan = await _mediator.Send(new BorrowBookCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPost("{id:guid}/return")]
    public async Task<ActionResult<LoanDto>> ReturnBook(Guid id, [FromBody] ReturnBookDto? dto = null)
    {
        var loan = await _mediator.Send(new ReturnBookCommand(id, dto));
        if (loan == null)
            return NotFound();

        return Ok(loan);
    }
}
