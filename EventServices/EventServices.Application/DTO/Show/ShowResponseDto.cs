using System;

namespace EventServices.Application.DTO.Show
{
    public class ShowResponseDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid ScreenId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
