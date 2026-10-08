using TicketSystem.WebAPI.Abstractions;
using WebAPIEgitimi.HastaKabulWebAPI.Abstractions;

namespace TicketSystem.WebAPI.Models
{
    public class TicketRating : Entity
    {
        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; } = default!;


        public Guid UserId { get; set; }

        public User User { get; set; } = default!;


        public int Score { get; set; }

        public string? Comment { get; set; }
    }
}
