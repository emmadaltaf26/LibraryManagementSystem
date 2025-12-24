using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetLoanByIdHandler : IRequestHandler<GetLoanByIdQuery, LoanDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLoanByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
