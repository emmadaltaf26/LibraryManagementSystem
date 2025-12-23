using Library.Domain.Common;

namespace Library.Domain.Entities;

/// <summary>
/// Represents an author in the library system
/// </summary>
public class Author : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Navigation property
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
