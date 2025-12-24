using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class LoanRepository : Repository<Loan>
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Loan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.Book)
            .Include(l => l.Member)
            .OrderByDescending(l => l.BorrowDate)
            .ToListAsync(cancellationToken);
    }
}
