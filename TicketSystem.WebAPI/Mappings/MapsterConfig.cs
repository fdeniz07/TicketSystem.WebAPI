using Mapster;
using TicketSystem.WebAPI.Abstractions;
using TicketSystem.WebAPI.DTOs.Ticket;
using TicketSystem.WebAPI.DTOs.TicketRating;
using TicketSystem.WebAPI.DTOs.TicketReply;
using TicketSystem.WebAPI.DTOs.User;
using TicketSystem.WebAPI.Models;

namespace TicketSystem.WebAPI.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<UserCreateDto, User>.NewConfig()
            .Map(
                dest => dest.Role,
                src => UserRole.FromValue(src.Role));

        TypeAdapterConfig<User, UserResponseDto>.NewConfig()
            .Map(
                dest => dest.Role,
                src => src.Role.Value);

        TypeAdapterConfig<TicketCreateDto, Ticket>.NewConfig();

        TypeAdapterConfig<Ticket, TicketResponseDto>.NewConfig()
            .Map(
                dest => dest.Status,
                src => src.Status.Value)
            .Map(
                dest => dest.CreatedByUserName,
                src => src.CreatedByUser.Name);

        TypeAdapterConfig<TicketReplyCreateDto, TicketReply>.NewConfig();

        TypeAdapterConfig<TicketRatingCreateDto, TicketRating>.NewConfig();
    }
}