using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class AircraftsDatum
    {
        public AircraftsDatum()
        {
            Flights = new HashSet<Flight>();
            Seats = new HashSet<Seat>();
        }

        public string AircraftCode { get; set; }
        public string Model { get; set; }
        public int Range { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
