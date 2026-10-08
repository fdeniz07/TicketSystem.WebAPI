using TicketSystem.WebAPI.Abstractions;

namespace TicketSystem.WebAPI.Models
{
    public sealed class Customer : User
    {
        public Customer()
        {
            Role = UserRole.Customer;
        }
    }
}
