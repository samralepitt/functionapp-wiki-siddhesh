using System;
using System.Collections.Generic;

namespace functionapp_wiki_siddhesh.Models
{
    public partial class Airport
    {
        public Airport()
        {
            Flights = new HashSet<Flight>();
        }

        public Guid AirportId { get; set; }
        public string AirportName { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
    }
}
