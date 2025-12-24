using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetOverdueLoansHandler : IRequestHandler<GetOverdueLoansQuery, IEnumerable<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOverdueLoansHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDto>> Handle(GetOverdueLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await _unitOfWork.Loans.FindAsync(l => l.ReturnDate == null && l.DueDate < DateTime.UtcNow, cancellationToken);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}
