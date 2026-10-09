using Carter;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.DTOs.Ticket;
using TicketSystem.WebAPI.Models;
using TS.Result;

namespace TicketSystem.WebAPI.Modules;

public sealed class TicketModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        var app = group
            .MapGroup("/tickets")
            .WithTags("Tickets")
            .RequireRateLimiting("fixed");


        ///////////// POST /tickets   \\\\\\\\\\\\\\\\
        app.MapPost("/", async (
            TicketCreateDto request,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var userExists = await dbContext.Users
                .AnyAsync(x => x.Id == request.CreatedByUserId, cancellationToken);

            if (!userExists)
            {
                var errorResponse = Result<object>.Failure(404, "Kullanıcı bulunamadı.");

                return Results.Ok(errorResponse);
            }

            var ticket = request.Adapt<Ticket>();

            dbContext.Tickets.Add(ticket);

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = ticket.Adapt<TicketResponseDto>();

            return Results.Ok(Result<TicketResponseDto>.Succeed(response));

        }).Produces<Result<TicketResponseDto>>();


        ///////////// GET /tickets   \\\\\\\\\\\\\\\\
        app.MapGet("/", async (
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var tickets = await dbContext.Tickets
                .AsNoTracking()
                .Include(x => x.CreatedByUser)
                .ToListAsync(cancellationToken);

            var response = tickets.Adapt<List<TicketResponseDto>>();

            return Results.Ok(Result<List<TicketResponseDto>>.Succeed(response));

        }).Produces<Result<List<TicketResponseDto>>>();


        ///////////// GET /tickets/{id}   \\\\\\\\\\\\\\\\
        app.MapGet("/{id:guid}", async (
            Guid id,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var ticket = await dbContext.Tickets
                .AsNoTracking()
                .Include(x => x.CreatedByUser)
                .Include(x => x.Replies)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (ticket is null)
            {
                var errorResponse = Result<object>.Failure(404, "Ticket bulunamadı.");

                return Results.Ok(errorResponse);
            }

            var messages = ticket.Replies
                .Select(x => new TicketMessageResponseDto(
                    x.UserId,
                    x.User.Name,
                    x.Message,
                    x.CreatedAt))
                .Append(new TicketMessageResponseDto(
                    ticket.CreatedByUserId,
                    ticket.CreatedByUser.Name,
                    ticket.Description,
                    ticket.CreatedAt))
                .OrderBy(x => x.CreatedAt)
                .ToList();

            var response = new TicketDetailResponseDto(
                ticket.Id,
                ticket.Title,
                ticket.Description,
                ticket.Status.Value,
                ticket.CreatedAt,
                messages);

            return Results.Ok(
                Result<TicketDetailResponseDto>.Succeed(response));

        }).Produces<Result<TicketDetailResponseDto>>();


        ///////////// PUT /tickets/{id}   \\\\\\\\\\\\\\\\
        app.MapPut("/{id:guid}", async (
            Guid id,
            TicketUpdateDto request,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (ticket is null)
            {
                var errorResponse = Result<object>.Failure(404, "Ticket bulunamadı.");

                return Results.Ok(errorResponse);
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                var errorResponse = Result<object>.Failure(400, "Kapalı bir ticket güncellenemez.");

                return Results.Ok(errorResponse);
            }

            ticket.Title = request.Title;
            ticket.Description = request.Description;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = ticket.Adapt<TicketResponseDto>();

            return Results.Ok(Result<TicketResponseDto>.Succeed(response));

        }).Produces<Result<TicketResponseDto>>();


        ///////////// PUT /tickets/{id}/close   \\\\\\\\\\\\\\\\
        app.MapPut("/{id:guid}/close", async (
            Guid id,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (ticket is null)
            {
                var errorResponse = Result<object>.Failure(404, "Ticket bulunamadı.");

                return Results.Ok(errorResponse);
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                var errorResponse = Result<object>.Failure(400, "Ticket zaten kapalı.");

                return Results.Ok(errorResponse);
            }

            ticket.Status = TicketStatus.Closed;
            ticket.UpdatedAt = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = ticket.Adapt<TicketResponseDto>();

            return Results.Ok(Result<TicketResponseDto>.Succeed(response));

        }).Produces<Result<TicketResponseDto>>();
    }
}