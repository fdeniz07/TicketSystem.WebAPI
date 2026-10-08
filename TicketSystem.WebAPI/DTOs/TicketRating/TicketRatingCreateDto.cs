namespace TicketSystem.WebAPI.DTOs.TicketRating
{
    public sealed record TicketRatingCreateDto(
        Guid UserId,
        int Score,
        string? Comment);
}
