using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public string Language { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
