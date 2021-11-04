using System;
using System.Collections.Generic;

#nullable disable

namespace HSA.Homework.Web
{
    public partial class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
        }

        public string BookRef { get; set; }
        public DateTime BookDate { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
