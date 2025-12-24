using Library.Domain.Common;

namespace Library.Domain.Entities;

/// <summary>
/// Represents a book in the library system
/// </summary>
public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PublishedYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    // Foreign keys
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }

    // Navigation properties
    public virtual Author Author { get; set; } = null!;
    public virtual Genre Genre { get; set; } = null!;
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
