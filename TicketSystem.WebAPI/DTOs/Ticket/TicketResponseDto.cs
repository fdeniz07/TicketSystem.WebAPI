namespace TicketSystem.WebAPI.DTOs.Ticket
{
    public sealed record TicketResponseDto(
        Guid Id,
        string Title,
        string Description,
        string Status,
        Guid CreatedByUserId,
        string CreatedByUserName,
        DateTimeOffset CreatedAt);
}
