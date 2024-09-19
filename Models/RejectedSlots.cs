using System;
using System.Collections.Generic;

namespace BowlingAlley.Models
{
    public class RejectedSlots
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public DateTime ReservedOn { get; set; }
        public int SlotId { get; set; }

        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }

        public string EmpName { get; set; }
    }
}
