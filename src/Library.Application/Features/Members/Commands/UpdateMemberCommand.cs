using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Members.Commands;

public record UpdateMemberCommand(Guid Id, UpdateMemberDto Member) : IRequest<MemberDto?>;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Member ID is required");

        RuleFor(x => x.Member.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.Member.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Member.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateMemberCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<MemberDto?> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            return null;

        // Check for duplicate email (excluding current member)
        var existingMember = await _unitOfWork.Members.FindAsync(
            m => m.Email.ToLower() == request.Member.Email.ToLower() && m.Id != request.Id,
            cancellationToken);

        if (existingMember.Any())
            throw new InvalidOperationException("A member with this email already exists");

        member.FirstName = request.Member.FirstName;
        member.LastName = request.Member.LastName;
        member.Email = request.Member.Email;
        member.PhoneNumber = request.Member.PhoneNumber;
        member.Address = request.Member.Address;
        member.IsActive = request.Member.IsActive;
        member.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Members.UpdateAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<MemberDto>(member);
    }
}
