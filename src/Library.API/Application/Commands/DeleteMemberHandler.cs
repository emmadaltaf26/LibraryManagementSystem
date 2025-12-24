using Library.Application.Common.Interfaces;
using Library.Domain.Enums;
using MediatR;

namespace Library.API.Application.Commands;

public class DeleteMemberHandler : IRequestHandler<DeleteMemberCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);
        if (member == null)
            return false;

        var activeLoans = await _unitOfWork.Loans.FindAsync(
            l => l.MemberId == request.Id && l.Status == LoanStatus.Active,
            cancellationToken);

        if (activeLoans.Any())
            throw new InvalidOperationException("Cannot delete a member with active loans");

        await _unitOfWork.Members.DeleteAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
