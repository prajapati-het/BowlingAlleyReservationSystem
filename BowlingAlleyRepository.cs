using Azure;
using BowlingAlley.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BowlingAlley
{
    public class BowlingAlleyRepository : IBowlingAlleyRepository
    {
        private readonly BowlingAlleyDBContext _context;
        private readonly ILogger<BowlingAlleyRepository> _logger;

        public BowlingAlleyRepository(BowlingAlleyDBContext context, ILogger<BowlingAlleyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int BookSlots(int SlotId, int EmpId, string CustomerName)
        {

            var slot = _context.BookingSlots.FirstOrDefault(s => s.SlotId == SlotId);

            var isSlotReserved = _context.Reservations.Any(r => r.SlotId == SlotId);

            var reservation = new Reservations
            {
                SlotId = SlotId,
                ReservedBy = EmpId,
                ReservedOn = DateTime.Now,
                Status = 1,
                CustomerName = CustomerName
            };

            try
            {
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return reservation.ReservationId;
            }
            catch (Exception ex)
            {
                throw;
            }
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

        public List<BookingSlots> AvailableSlots(DateTime date)
        {
            return _context.BookingSlots.ToList();
        }



    }
}
