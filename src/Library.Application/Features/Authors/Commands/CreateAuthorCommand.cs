using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Features.Authors.Commands;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }
}

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Author name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
    }
}

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAuthorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Biography = request.Biography,
            DateOfBirth = request.DateOfBirth,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Authors.AddAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuthorDto>(author);
    }
}
