using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.Application.Features.Books.Queries;

public class GetBookByIdQuery : IRequest<BookDto?>
{
    public Guid Id { get; set; }
}

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id, cancellationToken);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }
}
