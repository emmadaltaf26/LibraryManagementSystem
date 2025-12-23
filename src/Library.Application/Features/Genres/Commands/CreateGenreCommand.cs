using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Features.Genres.Commands;

public record CreateGenreCommand(CreateGenreDto Genre) : IRequest<GenreDto>;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreCommandValidator()
    {
        RuleFor(x => x.Genre.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
    }
}

public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGenreCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = _mapper.Map<Genre>(request.Genre);
        genre.Id = Guid.NewGuid();
        genre.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Genres.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GenreDto>(genre);
    }
}
