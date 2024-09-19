using System;


namespace BowlingAlley.Models
{
    public class ReservationRejections
    {
        public int ReservationId { get; set; }
        public Reservations Reservation { get; set; }

        public int EmpId { get; set; }
        public Roles Role { get; set; }

    }
}
