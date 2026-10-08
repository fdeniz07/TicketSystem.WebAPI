using Carter;
using Mapster;
using Microsoft.EntityFrameworkCore;
using TicketSystem.WebAPI.Abstractions;
using TicketSystem.WebAPI.Context;
using TicketSystem.WebAPI.DTOs.User;
using TicketSystem.WebAPI.Models;
using TS.Result;

namespace TicketSystem.WebAPI.Modules;

public sealed class UserModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder group)
    {
        var app = group
            .MapGroup("/users")
            .WithTags("Users")
            .RequireRateLimiting("fixed"); 


        ///////////// POST /users   \\\\\\\\\\\\\\\\
        app.MapPost("/", async (
            UserCreateDto request,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var emailExists = await dbContext.Users
                .AnyAsync(
                    x => x.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                var errorResponse = Result<object>.Failure(409, "Bu e-posta adresi zaten kullanılıyor.");
                return Results.Ok(errorResponse);
            }

            User user;

            switch (request.Role.ToLowerInvariant()) // Role'ü küçük harfe çevirerek kontrol ediyoruz
            {
                case "admin":
                    user = new Admin
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Role = UserRole.Admin
                    };
                    break;

                case "customer":
                    user = new Customer
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Role = UserRole.Customer
                    };
                    break;

                default:
                    var errorResponse = Result<object>.Failure(400, "Geçersiz rol. İzin verilen roller 'admin' ve 'customer'dır.");
                    return Results.BadRequest(errorResponse);
            }

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = user.Adapt<UserResponseDto>();

            return Results.Ok(Result<UserResponseDto>.Succeed(response));

        }).Produces<Result<UserResponseDto>>();


        ///////////// GET /users   \\\\\\\\\\\\\\\\
        app.MapGet("/", async (
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var users = await dbContext.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = users.Adapt<List<UserResponseDto>>();

            return Results.Ok(Result<List<UserResponseDto>>.Succeed(response));

        }).Produces<Result<List<UserResponseDto>>>();



        ///////////// GET /users/{id}   \\\\\\\\\\\\\\\\
        app.MapGet("/{id:guid}", async (
            Guid id,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var user = await dbContext.Users
                .AsNoTracking() // AsNoTracking, çünkü sadece okuma işlemi yapıyoruz
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user is null)
            {
                var errorResponse = Result<object>.Failure(404, "Kullanıcı bulunamadı.");
                return Results.Ok(errorResponse);
            }

            var response = user.Adapt<UserResponseDto>();

            return Results.Ok(Result<UserResponseDto>.Succeed(response));

        }).Produces<Result<UserResponseDto>>();




        ///////////// PUT /users/{id}   \\\\\\\\\\\\\\\\
        app.MapPut("/{id:guid}", async (
            Guid id,
            UserUpdateDto request,
            ApplicationDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user is null)
            {
                var errorResponse = Result<object>.Failure(404, "Kullanıcı bulunamadı.");
                return Results.Ok(errorResponse);
            }

            var emailExists = await dbContext.Users
                .AnyAsync(x => x.Email == request.Email && x.Id != id, cancellationToken);

            if (emailExists)
            {
                var errorResponse = Result<object>.Failure(409, "Bu e-posta adresi zaten kullanılıyor.");
                return Results.Ok(errorResponse);
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.UpdatedAt = DateTimeOffset.UtcNow;

            await dbContext.SaveChangesAsync(cancellationToken);

            var response = user.Adapt<UserResponseDto>();

            return Results.Ok(Result<UserResponseDto>.Succeed(response));

        }).Produces<Result<UserResponseDto>>();
    }
}