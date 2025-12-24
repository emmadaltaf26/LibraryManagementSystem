using AutoMapper;
using Library.Application.Common.Interfaces;
using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAuthorHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _unitOfWork.Authors.GetByIdAsync(request.Id, cancellationToken);
        if (author == null)
            return null;

        author.Name = request.Name;
        author.Biography = request.Biography;
        author.DateOfBirth = request.DateOfBirth;
        author.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Authors.UpdateAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AuthorDto>(author);
    }
}
