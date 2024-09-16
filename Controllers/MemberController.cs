using Microsoft.AspNetCore.Mvc;
using BowlingAlley.Models;
using System;
using System.Collections.Generic;

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
        public IActionResult ConfirmBooking(int SlotId, int EmpId, string CustomerName)
        {
            if (EmpId == 0 || string.IsNullOrEmpty(CustomerName))
            {
                return RedirectToAction("Exception");
            }

            try
            {
                int reservationId = _bowlingAlleyRepository.BookSlots(SlotId, EmpId, CustomerName);

                if (reservationId > 0)
                {
                    return RedirectToAction("Success");
                }

                return RedirectToAction("Exception");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Exception");
            }
        }

        public IActionResult RejectedSlots()
        {

            return View();
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
