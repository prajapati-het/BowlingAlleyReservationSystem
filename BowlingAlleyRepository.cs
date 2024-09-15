using BowlingAlley.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingAlley
{
    public class BowlingAlleyRepository : IBowlingAlleyRepository
    {
        private readonly BowlingAlleyDBContext _context;

        public BowlingAlleyRepository(BowlingAlleyDBContext context)
        {
            _context = context;
        }
        public int BookSlots(int SlotId, int EmpId)
        {
            var slot = _context.BookingSlots.FirstOrDefault(s => s.SlotId == SlotId);
            if (slot != null)
            {
                var reservation = new Reservations
                {
                    SlotId = SlotId,
                    ReservedBy = EmpId,
                    ReservedOn = DateTime.Now,
                    Status = 1
                };
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                return reservation.ReservationId;
            }
            return 0;
        }

        public string GetAdminName(int role)
        {
            var admin = _context.Roles.FirstOrDefault(r => r.RoleType && r.EmpId == role);
            return admin != null ? admin.EmpName : "Admin not found";
        }

        public List<Reservations> GetReservations()
        {
            return _context.Reservations.ToList();
        }

        public List<BookingSlots> GetBookingSlots()
        {
            return _context.BookingSlots.ToList();
        }
        public List<Reservations> GetReservationDetails()
        {
            return _context.Reservations
                .Select(r => new Reservations
                {
                    ReservationId = r.ReservationId,
                    ReservedBy = r.ReservedBy,
                    ReservedOn = r.ReservedOn,
                    Status = r.Status,
                    CustomerName = r.CustomerName,
                    Slot = r.Slot
                })
                .ToList();
        }
        public List<BookingSlots> AvailableSlots(DateTime dt)
        {
            return _context.BookingSlots
                .Where(slot => !slot.Reservations.Any())
                .ToList();
        }
    }
}
