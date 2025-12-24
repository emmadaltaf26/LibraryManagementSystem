using Library.Domain.Common;

namespace Library.Domain.Entities;

/// <summary>
/// Represents a book genre/category in the library system
/// </summary>
public class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation property
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
