using TicketSystem.WebAPI.Abstractions;

namespace TicketSystem.WebAPI.Models
{
    public sealed class Admin : User
    {
        public Admin()
        {
            Role = UserRole.Admin;
        }
    }
}
