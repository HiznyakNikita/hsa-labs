using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class Route
    {
        public string FlightNo { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalCity { get; set; }
        public string AircraftCode { get; set; }
        public TimeSpan? Duration { get; set; }
        public int[] DaysOfWeek { get; set; }
    }
}
