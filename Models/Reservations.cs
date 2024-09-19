using System.Data;
using System;
using System.Collections.Generic;

namespace BowlingAlley.Models
{
    public class Reservations
    {
        public int ReservationId { get; set; }
        public int ReservedBy { get; set; }
        public DateTime ReservedOn { get; set; }
        public int? Status { get; set; }
        public int SlotId { get; set; }
        public string CustomerName { get; set; }
        public BookingSlots Slot { get; set; }
        public Roles Role { get; set; }
        public ICollection<ReservationRejections> ReservationRejections { get; set; }

    }
}
