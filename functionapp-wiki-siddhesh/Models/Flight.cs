using System;
using System.Collections.Generic;

namespace functionapp_wiki_siddhesh.Models
{
    public partial class Flight
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? FlightDate { get; set; }
        public int? PartnerAirport { get; set; }
        public int? Gate { get; set; }

        public virtual Gate GateNavigation { get; set; }
        public virtual Airport PartnerAirportNavigation { get; set; }
    }
}
