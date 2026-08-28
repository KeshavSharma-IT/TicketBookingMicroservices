using System;

namespace EventServices.Application.DTO.Event
{
    public class EventResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public string Language { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
    }
}
