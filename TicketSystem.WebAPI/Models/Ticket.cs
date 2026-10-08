using TicketSystem.WebAPI.Abstractions;
using WebAPIEgitimi.HastaKabulWebAPI.Abstractions;

namespace TicketSystem.WebAPI.Models
{
    public sealed class Ticket : Entity
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = default!;

        public ICollection<TicketReply> Replies { get; set; } = [];

        public TicketRating? Rating { get; set; }
    }
}
