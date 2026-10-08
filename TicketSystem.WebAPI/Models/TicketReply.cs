using TicketSystem.WebAPI.Abstractions;
using WebAPIEgitimi.HastaKabulWebAPI.Abstractions;

namespace TicketSystem.WebAPI.Models
{
    public sealed class TicketReply : Entity
    {
        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; } = default!;

        public Guid UserId { get; set; }

        public User User { get; set; } = default!;

        public string Message { get; set; } = string.Empty;
    }
}
