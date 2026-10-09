namespace TicketSystem.WebAPI.DTOs.Ticket
{
    public sealed record TicketMessageResponseDto(
        Guid UserId,
        string UserName,
        string Message,
        DateTimeOffset CreatedAt);
}
