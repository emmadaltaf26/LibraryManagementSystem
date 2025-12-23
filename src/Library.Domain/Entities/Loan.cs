using Library.Domain.Common;
using Library.Domain.Enums;

namespace Library.Domain.Entities;

/// <summary>
/// Represents a book loan/borrowing record
/// </summary>
public class Loan : BaseEntity
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Active;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Book Book { get; set; } = null!;
    public virtual Member Member { get; set; } = null!;

    // Computed properties
    public bool IsOverdue => Status == LoanStatus.Active && DateTime.UtcNow > DueDate;
    public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;
}
