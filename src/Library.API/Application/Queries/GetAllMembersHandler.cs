using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAllMembersHandler : IRequestHandler<GetAllMembersQuery, IEnumerable<MemberDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMembersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _unitOfWork.Members.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<MemberDto>>(members);
    }
}
