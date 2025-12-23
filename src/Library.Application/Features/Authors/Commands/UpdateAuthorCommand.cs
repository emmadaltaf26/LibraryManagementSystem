using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Authors.Commands;

public class UpdateAuthorCommand : IRequest<AuthorDto?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Author ID is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
    }
}

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(request.Id, cancellationToken);
        if (author == null)
            return null;

        author.Name = request.Name;
        author.Biography = request.Biography;
        author.DateOfBirth = request.DateOfBirth;
        author.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuthorDto>(author);
    }
}
