using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Domain.Entities
{
    public class Screen
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }

        public string Name { get; set; }

        public int TotalSeats { get; set; }
        public bool IsActive { get; set; } = true;

        // navigation value
        public Venue venue { get; set; } = null;
    }
}
