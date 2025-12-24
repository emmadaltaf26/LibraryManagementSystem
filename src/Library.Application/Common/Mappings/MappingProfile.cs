using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;

namespace Library.Application.Common.Mappings;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Book mappings
        CreateMap<Book, BookDto>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(s => s.Author.Name))
            .ForMember(d => d.GenreName, opt => opt.MapFrom(s => s.Genre.Name));
        CreateMap<CreateBookDto, Book>()
            .ForMember(d => d.AvailableCopies, opt => opt.MapFrom(s => s.TotalCopies));
        CreateMap<UpdateBookDto, Book>();

        // Author mappings
        CreateMap<Author, AuthorDto>()
            .ForMember(d => d.BookCount, opt => opt.MapFrom(s => s.Books.Count));
        CreateMap<CreateAuthorDto, Author>();
        CreateMap<UpdateAuthorDto, Author>();

        // Genre mappings
        CreateMap<Genre, GenreDto>()
            .ForMember(d => d.BookCount, opt => opt.MapFrom(s => s.Books.Count));
        CreateMap<CreateGenreDto, Genre>();
        CreateMap<UpdateGenreDto, Genre>();

        // Member mappings
        CreateMap<Member, MemberDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.FullName))
            .ForMember(d => d.ActiveLoansCount, opt => opt.MapFrom(s => s.Loans.Count(l => l.Status == LoanStatus.Active)));
        CreateMap<CreateMemberDto, Member>()
            .ForMember(d => d.MembershipDate, opt => opt.MapFrom(s => DateTime.UtcNow))
            .ForMember(d => d.IsActive, opt => opt.MapFrom(s => true));
        CreateMap<UpdateMemberDto, Member>();

        // Loan mappings
        CreateMap<Loan, LoanDto>()
            .ForMember(d => d.BookTitle, opt => opt.MapFrom(s => s.Book.Title))
            .ForMember(d => d.BookISBN, opt => opt.MapFrom(s => s.Book.ISBN))
            .ForMember(d => d.MemberName, opt => opt.MapFrom(s => s.Member.FullName))
            .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.IsOverdue, opt => opt.MapFrom(s => s.IsOverdue))
            .ForMember(d => d.DaysOverdue, opt => opt.MapFrom(s => s.DaysOverdue));
    }
}
