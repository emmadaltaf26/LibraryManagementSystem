using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Members.Commands;

public class UpdateMemberCommand : IRequest<MemberDto?>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Member ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
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
