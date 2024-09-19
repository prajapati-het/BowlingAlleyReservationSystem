using System;

namespace BowlingAlley.Models
{
    public class RejectedSlotsViewModel
    {

        public int Id { get; set; }/*
        public int ReservationId { get; set; }
        public DateTime ReservedOn { get; set; }
        public int SlotId { get; set; }

        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }

        public string EmpName { get; set; }*/

        public Reservations Reservation { get; set; }

        public Roles Role { get; set; }

    }
}
