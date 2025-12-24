using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

/// <summary>
/// Member repository with eager loading of loans
/// </summary>
public class MemberRepository : Repository<Member>
{
    public MemberRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.Loans)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(m => m.Loans)
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync(cancellationToken);
    }
}
