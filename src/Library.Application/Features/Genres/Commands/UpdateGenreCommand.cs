using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Genres.Commands;

public class UpdateGenreCommand : IRequest<GenreDto?>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Genre ID is required");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Genre name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
    }
}

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, GenreDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGenreCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreDto?> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);
        if (genre == null)
            return null;

        genre.Name = request.Name;
        genre.Description = request.Description;
        genre.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Genres.UpdateAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GenreDto>(genre);
    }
}
