using Ardalis.SmartEnum;

namespace TicketSystem.WebAPI.Models
{
    public sealed class TicketStatus : SmartEnum<TicketStatus, string>
    {
        public static readonly TicketStatus Open = new("open", "Açık", true);

        public static readonly TicketStatus InProgress = new("in_progress", "İşlemde", true);

        public static readonly TicketStatus Closed = new("closed", "Kapalı", false);

        public bool IsActive { get; }

        private TicketStatus(string name, string value, bool isActive) : base(name, value)
        {
            IsActive = isActive;
        }
    }
}
