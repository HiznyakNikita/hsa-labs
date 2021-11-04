using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class Flight
    {
        public Flight()
        {
            TicketFlights = new HashSet<TicketFlight>();
        }

        public int FlightId { get; set; }
        public string FlightNo { get; set; }
        public DateTime ScheduledDeparture { get; set; }
        public DateTime ScheduledArrival { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public string Status { get; set; }
        public string AircraftCode { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime? ActualArrival { get; set; }

        public virtual AircraftsDatum AircraftCodeNavigation { get; set; }
        public virtual AirportsDatum ArrivalAirportNavigation { get; set; }
        public virtual AirportsDatum DepartureAirportNavigation { get; set; }
        public virtual ICollection<TicketFlight> TicketFlights { get; set; }
    }
}
