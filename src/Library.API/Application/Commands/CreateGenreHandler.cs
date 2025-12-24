using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;

namespace Library.API.Application.Commands;

public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, GenreDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateGenreHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreDto> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Genres.AddAsync(genre, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<GenreDto>(genre);
    }
}
