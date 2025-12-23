using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

/// <summary>
/// Genre repository with eager loading of books
/// </summary>
public class GenreRepository : Repository<Genre>
{
    public GenreRepository(LibraryDbContext context) : base(context)
    {
    }

    public override async Task<Genre?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Books)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Genre>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(g => g.Books)
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);
    }
}
