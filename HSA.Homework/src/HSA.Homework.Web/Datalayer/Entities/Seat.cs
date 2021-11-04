using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class Seat
    {
        public string AircraftCode { get; set; }
        public string SeatNo { get; set; }
        public string FareConditions { get; set; }

        public virtual AircraftsDatum AircraftCodeNavigation { get; set; }
    }
}
