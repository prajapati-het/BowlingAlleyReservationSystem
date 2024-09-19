using BowlingAlley.Models;
using System;
using System.Collections.Generic;

namespace BowlingAlley
{
    public interface IBowlingAlleyRepository
    {
        public int BookSlots(int Slotid, int Empid, string CustomerName);

        public void AddRejectedSlot(int reservationId, int empId);

        public Roles GetRoleByEmpId(int empId);

        public List<RejectedSlots> GetRejectedSlots(DateTime date, int slotid);

        public void AddSlot(TimeSpan st, TimeSpan et);

        public void DeleteSlot(int SlotId);

        public string GetAdminName(int role);

        public List<Reservations> GetReservations();

        public List<BookingSlots> GetBookingSlots();

        public List<Reservations> GetReservationDetails();

        public List<BookingSlots> AvailableSlots(DateTime dt);

    }
}
