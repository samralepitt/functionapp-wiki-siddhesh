using System;
using System.Collections.Generic;

namespace functionapp_wiki_siddhesh.Models
{
    public partial class Gate
    {
        public Gate()
        {
            Flights = new HashSet<Flight>();
        }

        public int GateId { get; set; }
        public string GateName { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
    }
}
