namespace TicketSystem.WebAPI.DTOs.TicketReply
{
    public sealed record TicketReplyResponseDto(
        Guid Id,
        Guid TicketId,
        Guid UserId,
        string UserName,
        string Message,
        DateTimeOffset CreatedAt);
}
