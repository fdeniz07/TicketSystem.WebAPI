using Carter;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.DTOs.TicketReply;
using TicketSystem.WebAPI.Models;
using TS.Result;

namespace TicketSystem.WebAPI.Modules;

public sealed class TicketReplyModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        var app = group
            .MapGroup("/tickets/{ticketId:guid}/replies")
            .WithTags("Ticket Replies")
            .RequireRateLimiting("fixed");


        ///////////// POST /tickets/{ticketId}/replies   \\\\\\\\\\\\\\\\
        app.MapPost("/", async (
            Guid ticketId,
            TicketReplyCreateDto request,
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

            if (ticket.Status == TicketStatus.Closed)
            {
                var errorResponse = Result<object>.Failure(400, "Kapalı bir ticket'a cevap verilemez.");

                return Results.Ok(errorResponse);
            }

            var userExists = await dbContext.Users
                .AnyAsync(x => x.Id == request.UserId, cancellationToken);

            if (!userExists)
            {
                var errorResponse = Result<object>.Failure(404, "Kullanıcı bulunamadı.");

                return Results.Ok(errorResponse);
            }

            var reply = request.Adapt<TicketReply>();

            reply.TicketId = ticketId;

            dbContext.TicketReplies.Add(reply);

            ticket.Status = TicketStatus.InProgress;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(
                Result<object>.Succeed(new
                {
                    Message = "Cevap başarıyla eklendi.",
                    ReplyId = reply.Id
                }));

        }).Produces<Result<object>>();


        ///////////// GET /tickets/{ticketId}/replies   \\\\\\\\\\\\\\\\
        app.MapGet("/", async (
            Guid ticketId,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var ticketExists = await dbContext.Tickets
                .AnyAsync(
                    x => x.Id == ticketId,
                    cancellationToken);

            if (!ticketExists)
            {
                var errorResponse = Result<object>.Failure(404, "Ticket bulunamadı.");

                return Results.Ok(errorResponse);
            }

            var replies = await dbContext.TicketReplies
                .AsNoTracking()
                .Where(x => x.TicketId == ticketId)
                .Include(x => x.User)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

            var response = replies.Adapt<List<TicketReply>>();

            return Results.Ok(Result<List<TicketReply>>.Succeed(response));

        }).Produces<Result<List<TicketReply>>>();
    }
}