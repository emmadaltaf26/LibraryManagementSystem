using AutoMapper;
using FluentValidation;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Genres.Commands;

public record UpdateGenreCommand(Guid Id, UpdateGenreDto Genre) : IRequest<GenreDto?>;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
    public UpdateGenreCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Genre ID is required");
        RuleFor(x => x.Genre.Name)
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

        genre.Name = request.Genre.Name;
        genre.Description = request.Genre.Description;
        genre.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Genres.UpdateAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GenreDto>(genre);
    }
}
