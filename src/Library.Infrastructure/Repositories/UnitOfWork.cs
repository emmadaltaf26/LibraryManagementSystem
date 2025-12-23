using Library.Application.Common.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;

namespace Library.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions across repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    private IRepository<Book>? _books;
    private IRepository<Author>? _authors;
    private IRepository<Genre>? _genres;
    private IRepository<Member>? _members;
    private IRepository<Loan>? _loans;

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
    }

    public IRepository<Book> Books => _books ??= new BookRepository(_context);
    public IRepository<Author> Authors => _authors ??= new AuthorRepository(_context);
    public IRepository<Genre> Genres => _genres ??= new GenreRepository(_context);
    public IRepository<Member> Members => _members ??= new MemberRepository(_context);
    public IRepository<Loan> Loans => _loans ??= new LoanRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
