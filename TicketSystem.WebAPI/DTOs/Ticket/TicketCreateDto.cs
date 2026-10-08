namespace TicketSystem.WebAPI.DTOs.Ticket
{
    public sealed record TicketCreateDto(
        string Title,
        string Description,
        Guid CreatedByUserId);
}
