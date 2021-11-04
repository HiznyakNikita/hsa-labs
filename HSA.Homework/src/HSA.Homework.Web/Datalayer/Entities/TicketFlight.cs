using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class TicketFlight
    {
        public string TicketNo { get; set; }
        public int FlightId { get; set; }
        public string FareConditions { get; set; }
        public decimal Amount { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Ticket TicketNoNavigation { get; set; }
        public virtual BoardingPass BoardingPass { get; set; }
    }
}
