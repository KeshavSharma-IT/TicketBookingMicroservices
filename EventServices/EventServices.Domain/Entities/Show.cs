using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Domain.Entities
{
    public class Show
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid ScreenId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public Event Event { get; set; } = null!;
        public Screen Screen { get; set; } = null!;
    }
}
