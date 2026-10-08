namespace TicketSystem.WebAPI.DTOs.User
{
    public sealed record UserResponseDto(
        Guid Id,
        string Name,
        string Email,
        string Role,
        DateTimeOffset CreatedAt);
}
