using Library.Domain.Entities;
using Library.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.BorrowDate)
            .IsRequired();

        builder.Property(l => l.DueDate)
            .IsRequired();

        builder.Property(l => l.Status)
            .IsRequired()
            .HasDefaultValue(LoanStatus.Active);

        builder.Property(l => l.Notes)
            .HasMaxLength(1000);

        // Ignore computed properties
        builder.Ignore(l => l.IsOverdue);
        builder.Ignore(l => l.DaysOverdue);

        // Relationships
        builder.HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.DueDate);
        builder.HasIndex(l => new { l.BookId, l.MemberId, l.Status });
    }
}
