using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand, MemberDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMemberHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberDto?> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            return null;

        var existingMember = await _unitOfWork.Members.FindAsync(
            m => m.Email.ToLower() == request.Email.ToLower() && m.Id != request.Id,
            cancellationToken);

        if (existingMember.Any())
            throw new InvalidOperationException("A member with this email already exists");

        member.FirstName = request.FirstName;
        member.LastName = request.LastName;
        member.Email = request.Email;
        member.PhoneNumber = request.PhoneNumber;
        member.Address = request.Address;
        member.IsActive = request.IsActive;
        member.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Members.UpdateAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MemberDto>(member);
    }
}
