using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, GenreDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGenreByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
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
