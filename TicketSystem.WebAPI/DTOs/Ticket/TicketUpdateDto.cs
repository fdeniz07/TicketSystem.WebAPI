namespace TicketSystem.WebAPI.DTOs.Ticket
{
    public sealed record TicketUpdateDto(
        string Title,
        string Description);
}
