using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Queries;

public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGenresHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _unitOfWork.Genres.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GenreDto>>(genres);
    }
}
