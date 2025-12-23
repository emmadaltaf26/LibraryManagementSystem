using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Authors.Commands;

public record UpdateAuthorCommand(Guid Id, UpdateAuthorDto Author) : IRequest<AuthorDto?>;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Author ID is required");
        RuleFor(x => x.Author.Name)
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

        author.Name = request.Author.Name;
        author.Biography = request.Author.Biography;
        author.DateOfBirth = request.Author.DateOfBirth;
        author.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuthorDto>(author);
    }
}
