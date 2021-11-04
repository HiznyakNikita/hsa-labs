using System;
using System.Collections.Generic;
using NpgsqlTypes;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class AirportsDatum
    {
        public AirportsDatum()
        {
            FlightArrivalAirportNavigations = new HashSet<Flight>();
            FlightDepartureAirportNavigations = new HashSet<Flight>();
        }

        public string AirportCode { get; set; }
        public string AirportName { get; set; }
        public string City { get; set; }
        public NpgsqlPoint Coordinates { get; set; }
        public string Timezone { get; set; }

        public virtual ICollection<Flight> FlightArrivalAirportNavigations { get; set; }
        public virtual ICollection<Flight> FlightDepartureAirportNavigations { get; set; }
    }
}
