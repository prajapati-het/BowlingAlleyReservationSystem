using BowlingAlley.Models;
using System;
using System.Collections.Generic;

namespace BowlingAlley
{
    public interface IBowlingAlleyRepository
    {
        public int BookSlots(int Slotid, int Empid, string CustomerName);

        //public     GetRejectedSlots();

        public string GetAdminName(int role);

        public List<Reservations> GetReservations();

        public List<BookingSlots> GetBookingSlots();

        public List<Reservations> GetReservationDetails();

        public List<BookingSlots> AvailableSlots(DateTime dt);

    }
}
