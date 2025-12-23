using Library.Domain.Entities;

namespace Library.Application.Common.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<Book> Books { get; }
    IRepository<Author> Authors { get; }
    IRepository<Genre> Genres { get; }
    IRepository<Member> Members { get; }
    IRepository<Loan> Loans { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
