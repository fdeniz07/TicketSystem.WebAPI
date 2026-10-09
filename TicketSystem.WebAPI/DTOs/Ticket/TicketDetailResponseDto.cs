namespace TicketSystem.WebAPI.DTOs.Ticket
{
    public sealed record TicketDetailResponseDto(
        Guid Id,
        string Title,
        string Description,
        string Status,
        DateTimeOffset CreatedAt,
        List<TicketMessageResponseDto> Messages);
}
