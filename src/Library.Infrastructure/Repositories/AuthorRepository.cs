using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

/// <summary>
/// Author repository with eager loading of books
/// </summary>
public class AuthorRepository : Repository<Author>
{
    public AuthorRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<Author?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Books)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);
    }
}
