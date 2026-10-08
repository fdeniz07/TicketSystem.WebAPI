using Carter;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.DTOs.TicketRating;
using TicketSystem.WebAPI.Models;
using TS.Result;

namespace TicketSystem.WebAPI.Modules;

public sealed class TicketRatingModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        var app = group
            .MapGroup("/tickets/{ticketId:guid}/rating")
            .WithTags("Ticket Rating");


        ///////////// POST /tickets/{ticketId}/rating   \\\\\\\\\\\\\\\\
        app.MapPost("/", async (
            Guid ticketId,
            TicketRatingCreateDto request,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(x => x.Id == ticketId, cancellationToken);

            if (ticket is null)
            {
                var errorResponse = Result<object>.Failure(404, "Ticket bulunamadı.");

                return Results.Ok(errorResponse);
            }

            var userExists = await dbContext.Users
                .AnyAsync(x => x.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                var errorResponse = Result<object>.Failure(404, "Kullanıcı bulunamadı.");

                return Results.Ok(errorResponse);
            }

            if (ticket.CreatedByUserId != request.UserId)
            {
                var errorResponse = Result<object>.Failure(403, "Bu ticket için sadece ticket'ı oluşturan kullanıcı değerlendirme yapabilir.");

                return Results.Ok(errorResponse);
            }

            if (request.Score < 1 || request.Score > 5)
            {
                var errorResponse = Result<object>.Failure(400, "Puan 1 ile 5 arasında olmalıdır.");

                return Results.Ok(errorResponse);
            }

            var ratingExists = await dbContext.TicketRatings
                .AnyAsync(x => x.TicketId == ticketId, cancellationToken);

            if (ratingExists)
            {
                var errorResponse = Result<object>.Failure(409, "Bu ticket için daha önce değerlendirme yapılmış.");

                return Results.Ok(errorResponse);
            }

            var rating = request.Adapt<TicketRating>();

            rating.TicketId = ticketId;

            dbContext.TicketRatings.Add(rating);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(
                Result<object>.Succeed(new
                {
                    Message = "Değerlendirme başarıyla kaydedildi.",
                    RatingId = rating.Id
                }));

        }).Produces<Result<object>>();


        ///////////// GET /tickets/{ticketId}/rating   \\\\\\\\\\\\\\\\
        app.MapGet("/", async (
            Guid ticketId,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var rating = await dbContext.TicketRatings
                .AsNoTracking()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.TicketId == ticketId, cancellationToken);

            if (rating is null)
            {
                var errorResponse = Result<object>.Failure(404, "Bu ticket için henüz değerlendirme yapılmamış.");

                return Results.Ok(errorResponse);
            }

            var response = rating.Adapt<TicketRatingResponseDto>();

            return Results.Ok(Result<TicketRatingResponseDto>.Succeed(response));

        }).Produces<Result<TicketRatingResponseDto>>();
    }
}