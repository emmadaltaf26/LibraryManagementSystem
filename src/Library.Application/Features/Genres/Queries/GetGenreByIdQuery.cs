using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Genres.Queries;

public class GetGenreByIdQuery : IRequest<GenreDto?>
{
    public Guid Id { get; set; }
}

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, GenreDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGenreByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreDto?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork.Genres.GetByIdAsync(request.Id, cancellationToken);
        return genre == null ? null : _mapper.Map<GenreDto>(genre);
    }
}
