using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class BoardingPass
    {
        public string TicketNo { get; set; }
        public int FlightId { get; set; }
        public int BoardingNo { get; set; }
        public string SeatNo { get; set; }

        public virtual TicketFlight TicketFlight { get; set; }
    }
}
