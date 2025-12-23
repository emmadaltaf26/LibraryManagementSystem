using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Loans.Queries;

public class GetLoanByIdQuery : IRequest<LoanDto?>
{
    public Guid Id { get; set; }
}

public class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, LoanDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoanByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoanDto?> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var loan = await _unitOfWork.Loans.GetByIdAsync(request.Id, cancellationToken);
        return loan == null ? null : _mapper.Map<LoanDto>(loan);
    }
}
