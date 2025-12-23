using Library.Domain.Enums;

namespace Library.Application.DTOs;

public class LoanDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookISBN { get; set; } = string.Empty;
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }
}

public class BorrowBookDto
{
    public Guid BookId { get; set; }
    public Guid MemberId { get; set; }
    public int LoanDays { get; set; } = 14; // Default 14 days loan period
    public string? Notes { get; set; }
}

public class ReturnBookDto
{
    public string? Notes { get; set; }
}
