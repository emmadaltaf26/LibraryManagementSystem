using Library.Domain.Common;

namespace Library.Domain.Entities;

/// <summary>
/// Represents a library member who can borrow books
/// </summary>
public class Member : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime MembershipDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    // Computed property
    public string FullName => $"{FirstName} {LastName}";
}
