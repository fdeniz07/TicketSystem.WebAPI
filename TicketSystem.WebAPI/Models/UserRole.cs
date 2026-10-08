using Ardalis.SmartEnum;

namespace TicketSystem.WebAPI.Models
{
    public sealed class UserRole : SmartEnum<UserRole, string>
    {
        public static readonly UserRole Admin = new("admin", "Yönetici");

        public static readonly UserRole Customer = new("customer", "Müşteri");

        private UserRole(string name, string value) : base(name, value)
        {
        }
    }
}
