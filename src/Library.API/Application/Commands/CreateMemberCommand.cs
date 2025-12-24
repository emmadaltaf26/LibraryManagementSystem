using Library.Application.DTOs;
using MediatR;

namespace Library.API.Application.Commands;

public class CreateMemberCommand : IRequest<MemberDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
}
