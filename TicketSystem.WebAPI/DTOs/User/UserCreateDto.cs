namespace TicketSystem.WebAPI.DTOs.User
{
    public sealed record UserCreateDto(
        string Name,
        string Email,
        string Role);
}
