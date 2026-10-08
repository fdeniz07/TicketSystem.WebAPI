using TicketSystem.WebAPI.Models;
using WebAPIEgitimi.HastaKabulWebAPI.Abstractions;

namespace TicketSystem.WebAPI.Abstractions
{
    public abstract class User : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;

        public ICollection<Ticket> CreatedTickets { get; set; } = [];
        public ICollection<TicketReply> TicketReplies { get; set; } = [];
        public ICollection<TicketRating> TicketRatings { get; set; } = [];
    }
}
