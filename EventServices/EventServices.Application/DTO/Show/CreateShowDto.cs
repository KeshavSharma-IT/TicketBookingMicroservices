using System;

namespace EventServices.Application.DTO.Show
{
    public class CreateShowDto
    {
        public Guid EventId { get; set; }
        public Guid ScreenId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
    }
}
