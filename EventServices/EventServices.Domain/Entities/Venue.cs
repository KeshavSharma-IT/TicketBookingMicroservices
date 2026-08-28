using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Domain.Entities
{
    public class Venue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string City { get; set; }
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;

        //navigation property

        public ICollection<Screen> Screens { get; set; } = new List<Screen>();
    }
}
