namespace TicketSystem.WebAPI.DTOs.TicketReply
{
    public sealed record TicketReplyCreateDto(
        Guid UserId,
        string Message);
}
