using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Loans.Queries;

public record GetLoansByMemberQuery(Guid MemberId) : IRequest<IEnumerable<LoanDto>>;

public class GetLoansByMemberQueryHandler : IRequestHandler<GetLoansByMemberQuery, IEnumerable<LoanDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoansByMemberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LoanDto>> Handle(GetLoansByMemberQuery request, CancellationToken cancellationToken)
    {
        var loans = await _unitOfWork.Loans.FindAsync(l => l.MemberId == request.MemberId, cancellationToken);
        return _mapper.Map<IEnumerable<LoanDto>>(loans);
    }
}
