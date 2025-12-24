using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, GenreDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGenreHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
