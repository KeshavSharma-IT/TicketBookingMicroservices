using System;

namespace EventServices.Application.DTO.Venue
{
    public class ScreenResponseDto
    {
        public Guid Id { get; set; }
        public Guid VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public bool IsActive { get; set; }
    }
}
