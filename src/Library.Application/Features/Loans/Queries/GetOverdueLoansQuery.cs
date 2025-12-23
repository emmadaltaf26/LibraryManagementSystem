using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Enums;
using MediatR;

namespace Library.Application.Features.Loans.Queries;

public record GetOverdueLoansQuery : IRequest<IEnumerable<LoanDto>>;

public class GetOverdueLoansQueryHandler : IRequestHandler<GetOverdueLoansQuery, IEnumerable<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOverdueLoansQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDto>> Handle(GetOverdueLoansQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var overdueLoans = await _unitOfWork.Loans.FindAsync(
            l => l.Status == LoanStatus.Active && l.DueDate < now,
            cancellationToken);

        return _mapper.Map<IEnumerable<LoanDto>>(overdueLoans);
    }
}
