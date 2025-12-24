using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Data;

public static class LibraryDbContextSeed
{
    public static async Task SeedAsync(LibraryDbContext context, ILogger logger)
    {
        try
        {
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            if (!await context.Genres.AnyAsync())
            {
                var genres = GetPreconfiguredGenres();
                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} genres", genres.Count);
            }

            if (!await context.Authors.AnyAsync())
            {
                var authors = GetPreconfiguredAuthors();
                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} authors", authors.Count);
            }

            if (!await context.Books.AnyAsync())
            {
                var genres = await context.Genres.ToListAsync();
                var authors = await context.Authors.ToListAsync();
                var books = GetPreconfiguredBooks(authors, genres);
                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} books", books.Count);
            }

            if (!await context.Members.AnyAsync())
            {
                var members = GetPreconfiguredMembers();
                await context.Members.AddRangeAsync(members);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} members", members.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static List<Genre> GetPreconfiguredGenres()
    {
        return new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Fiction", Description = "Literary works of imagination", CreatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Non-Fiction", Description = "Factual literature", CreatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Science Fiction", Description = "Speculative fiction with scientific themes", CreatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Mystery", Description = "Crime and detective stories", CreatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Biography", Description = "Life stories of real people", CreatedAt = DateTime.UtcNow },
            new Genre { Id = Guid.NewGuid(), Name = "Technology", Description = "Books about technology and computing", CreatedAt = DateTime.UtcNow }
        };
    }

    private static List<Author> GetPreconfiguredAuthors()
    {
        return new List<Author>
        {
            new Author { Id = Guid.NewGuid(), Name = "George Orwell", Biography = "English novelist and essayist", DateOfBirth = new DateTime(1903, 6, 25), CreatedAt = DateTime.UtcNow },
            new Author { Id = Guid.NewGuid(), Name = "Jane Austen", Biography = "English novelist known for romantic fiction", DateOfBirth = new DateTime(1775, 12, 16), CreatedAt = DateTime.UtcNow },
            new Author { Id = Guid.NewGuid(), Name = "Isaac Asimov", Biography = "American writer and professor of biochemistry", DateOfBirth = new DateTime(1920, 1, 2), CreatedAt = DateTime.UtcNow },
            new Author { Id = Guid.NewGuid(), Name = "Agatha Christie", Biography = "English writer known for detective novels", DateOfBirth = new DateTime(1890, 9, 15), CreatedAt = DateTime.UtcNow },
            new Author { Id = Guid.NewGuid(), Name = "Robert C. Martin", Biography = "American software engineer and author", DateOfBirth = new DateTime(1952, 12, 5), CreatedAt = DateTime.UtcNow }
        };
    }

    private static List<Book> GetPreconfiguredBooks(List<Author> authors, List<Genre> genres)
    {
        var fiction = genres.First(g => g.Name == "Fiction");
        var scifi = genres.First(g => g.Name == "Science Fiction");
        var mystery = genres.First(g => g.Name == "Mystery");
        var tech = genres.First(g => g.Name == "Technology");

        var orwell = authors.First(a => a.Name == "George Orwell");
        var austen = authors.First(a => a.Name == "Jane Austen");
        var asimov = authors.First(a => a.Name == "Isaac Asimov");
        var christie = authors.First(a => a.Name == "Agatha Christie");
        var martin = authors.First(a => a.Name == "Robert C. Martin");

        return new List<Book>
        {
            new Book { Id = Guid.NewGuid(), Title = "1984", ISBN = "978-0451524935", Description = "Dystopian social science fiction", PublishedYear = 1949, AuthorId = orwell.Id, GenreId = fiction.Id, TotalCopies = 5, AvailableCopies = 5, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Animal Farm", ISBN = "978-0451526342", Description = "Allegorical novella", PublishedYear = 1945, AuthorId = orwell.Id, GenreId = fiction.Id, TotalCopies = 3, AvailableCopies = 3, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Pride and Prejudice", ISBN = "978-0141439518", Description = "Romantic novel of manners", PublishedYear = 1813, AuthorId = austen.Id, GenreId = fiction.Id, TotalCopies = 4, AvailableCopies = 4, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Foundation", ISBN = "978-0553293357", Description = "Science fiction novel", PublishedYear = 1951, AuthorId = asimov.Id, GenreId = scifi.Id, TotalCopies = 3, AvailableCopies = 3, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "I, Robot", ISBN = "978-0553382563", Description = "Collection of science fiction short stories", PublishedYear = 1950, AuthorId = asimov.Id, GenreId = scifi.Id, TotalCopies = 2, AvailableCopies = 2, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Murder on the Orient Express", ISBN = "978-0062693662", Description = "Detective novel featuring Hercule Poirot", PublishedYear = 1934, AuthorId = christie.Id, GenreId = mystery.Id, TotalCopies = 4, AvailableCopies = 4, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Clean Code", ISBN = "978-0132350884", Description = "A handbook of agile software craftsmanship", PublishedYear = 2008, AuthorId = martin.Id, GenreId = tech.Id, TotalCopies = 6, AvailableCopies = 6, CreatedAt = DateTime.UtcNow },
            new Book { Id = Guid.NewGuid(), Title = "Clean Architecture", ISBN = "978-0134494166", Description = "A guide to software structure and design", PublishedYear = 2017, AuthorId = martin.Id, GenreId = tech.Id, TotalCopies = 4, AvailableCopies = 4, CreatedAt = DateTime.UtcNow }
        };
    }

    private static List<Member> GetPreconfiguredMembers()
    {
        return new List<Member>
        {
            new Member { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-0101", Address = "123 Main St", MembershipDate = DateTime.UtcNow.AddMonths(-6), IsActive = true, CreatedAt = DateTime.UtcNow },
            new Member { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "555-0102", Address = "456 Oak Ave", MembershipDate = DateTime.UtcNow.AddMonths(-3), IsActive = true, CreatedAt = DateTime.UtcNow },
            new Member { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", PhoneNumber = "555-0103", Address = "789 Pine Rd", MembershipDate = DateTime.UtcNow.AddMonths(-1), IsActive = true, CreatedAt = DateTime.UtcNow }
        };
    }
}
