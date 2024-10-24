using Microsoft.AspNetCore.Mvc;
using BowlingAlley.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingAlley.Controllers
{
    public class MemberController : Controller
    {
        private readonly IBowlingAlleyRepository _bowlingAlleyRepository;

        public MemberController(IBowlingAlleyRepository bowlingAlleyRepository)
        {
            _bowlingAlleyRepository = bowlingAlleyRepository;
        }

        public IActionResult GetFreeSlots()
        {
            var freeSlots = _bowlingAlleyRepository.AvailableSlots(new DateTime());
            return View(freeSlots);
        }


        [HttpGet]
        public IActionResult BookSlots(int SlotId)
        {
            var slot = _bowlingAlleyRepository.GetBookingSlots().Find(s => s.SlotId == SlotId);

            if (slot == null)
            {
                return RedirectToAction("Exception");
            }

            return View(slot);
        }

        [HttpPost]
        public IActionResult ConfirmBooking(int SlotId, string CustomerName, int EmpId)
        {
            var existingReservation = _bowlingAlleyRepository.GetReservations()
                .FirstOrDefault(r => r.SlotId == SlotId && r.Status == 1);

            

            if (existingReservation != null)
            {
                var employeeRole = _bowlingAlleyRepository.GetRoleByEmpId(existingReservation.ReservedBy);


                var bookedSlot = _bowlingAlleyRepository.GetBookingSlots().FirstOrDefault(s => s.SlotId == SlotId);

                var rejectedSlot = new RejectedSlots()
                {
                    EmpName = employeeRole.EmpName,
                    ReservationId = existingReservation.ReservationId,
                    ReservedOn = existingReservation.ReservedOn,
                    SlotEndTime = bookedSlot.SlotEndTime,
                    SlotId = bookedSlot.SlotId,
                    SlotStartTime = bookedSlot.SlotStartTime,
                };

                _bowlingAlleyRepository.AddRejectedSlot(existingReservation.ReservationId, EmpId);

                return RedirectToAction("RejectedSlots", new { date = existingReservation.ReservedOn, slotId = SlotId });
            }
            else
            {
                _bowlingAlleyRepository.BookSlots(SlotId, EmpId, CustomerName);

                return RedirectToAction("Success");
            }
        }


        public IActionResult RejectedSlots(DateTime date, int slotId)
        {
            var rejectedSlots = _bowlingAlleyRepository.GetRejectedSlots(date, slotId);
            return View(rejectedSlots);
        }


        [HttpGet]
        public IActionResult AddSlot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSlot(string SST, string SET)
        {
            try
            {
                TimeSpan startTime = TimeSpan.Parse(SST);
                TimeSpan endTime = TimeSpan.Parse(SET);

                var existingSlot = _bowlingAlleyRepository.GetBookingSlots()
                    .FirstOrDefault(s => s.SlotStartTime == startTime && s.SlotEndTime == endTime);

                if (existingSlot != null)
                {
                    ViewBag.ErrorMessage = "This slot already exists.";
                    return View();
                }

                _bowlingAlleyRepository.AddSlot(startTime, endTime);

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Exception");
            }
        }

        [HttpPost]
        public IActionResult DeleteSlot(int SlotId)
        {
            try
            {
                var slot = _bowlingAlleyRepository.GetBookingSlots().FirstOrDefault(s => s.SlotId == SlotId);

                if (slot == null)
                {
                    ViewBag.ErrorMessage = "Slot not found.";
                    return View();
                }

                _bowlingAlleyRepository.DeleteSlot(SlotId);

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Exception");
            }
        }

        [HttpGet]
        public IActionResult ManageSlots()
        {
            var slots = _bowlingAlleyRepository.GetBookingSlots();
            return View(slots);
        }


        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Exception()
        {
            return View();
        }
    }
}
