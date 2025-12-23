using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Members.Queries;

public class GetMemberByIdQuery : IRequest<MemberDto?>
{
    public Guid Id { get; set; }
}

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMemberByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberDto?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);
        return member == null ? null : _mapper.Map<MemberDto>(member);
    }
}
