
namespace TicketSystem.WebAPI.DTOs.TicketRating;

public sealed record TicketRatingResponseDto(
    Guid Id,
    Guid TicketId,
    Guid UserId,
    string UserName,
    int Score,
    string? Comment,
    DateTimeOffset CreatedAt);

