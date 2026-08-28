namespace EventServices.Application.DTO.Venue
{
    public class CreateScreenDto
    {
        public string Name { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
    }
}
