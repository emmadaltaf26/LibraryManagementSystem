using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.Description)
            .HasMaxLength(2000);

        builder.Property(b => b.PublishedYear)
            .IsRequired();

        builder.Property(b => b.TotalCopies)
            .IsRequired();

        builder.Property(b => b.AvailableCopies)
            .IsRequired();

        // Relationships
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(b => b.ISBN);
        builder.HasIndex(b => b.Title);
    }
}
