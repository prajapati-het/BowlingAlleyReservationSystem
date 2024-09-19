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


        public void AddRejectedSlot(int reservationId, int empId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation != null)
            {
                var reservationRejection = new ReservationRejections
                {
                    ReservationId = reservation.ReservationId,
                    EmpId = empId
                };

                _context.ReservationRejections.Add(reservationRejection);

                
                var rejectedSlot = new RejectedSlots
                {
                    ReservationId = reservation.ReservationId,
                    ReservedOn = reservation.ReservedOn,
                    SlotId = reservation.SlotId,
                    SlotStartTime = reservation.Slot.SlotStartTime, 
                    SlotEndTime = reservation.Slot.SlotEndTime,
                    EmpName = _context.Roles.FirstOrDefault(r => r.EmpId == empId)?.EmpName
                };

                _context.RejectedSlots.Add(rejectedSlot);

                try
                {
                    _context.SaveChanges();
                }
                catch(Exception ex)
                {

                }
            }
            else
            {
                throw new Exception("Reservation not found.");
            }
        }





        public Roles GetRoleByEmpId(int empId)
        {
            return _context.Roles.FirstOrDefault(r => r.EmpId == empId);
        }




        public List<RejectedSlots> GetRejectedSlots(DateTime date, int slotid)
        {
            // Fetch rejected slots from RejectedSlots table based on the given slot ID and date
            var rejectedSlots = _context.RejectedSlots
                .Where(rs => rs.SlotId == slotid && rs.ReservedOn.Date == date.Date)
                .ToList();

            foreach (var rejectedSlot in rejectedSlots)
            {
                // Get reservation details from the Reservations table
                var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == rejectedSlot.ReservationId);

                // Get employee details (Role) who originally reserved the slot
                var employeeRole = _context.Roles.FirstOrDefault(r => r.EmpId == reservation.ReservedBy);

                // Populate additional properties in the RejectedSlots model, if needed
                rejectedSlot.EmpName = employeeRole?.EmpName; // Assuming EmpName is a field in RejectedSlots

                // No need to create a view model; add the rejectedSlot directly to the list
            }

            return rejectedSlots;
        }



        public void AddSlot(TimeSpan st, TimeSpan et)
        {
            BookingSlots _bs = new BookingSlots()
            {
                SlotStartTime = st,
                SlotEndTime = et
            };

            _context.BookingSlots.Add(_bs);
            _context.SaveChanges();

        }

        public void DeleteSlot(int SlotId)
        {
            var slot = _context.BookingSlots.FirstOrDefault(s => s.SlotId == SlotId);
            if (slot != null)
            {
                _context.BookingSlots.Remove(slot);
                _context.SaveChanges();
            }
        }



    }
}
