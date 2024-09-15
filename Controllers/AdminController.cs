using Microsoft.AspNetCore.Mvc;

namespace BowlingAlley.Controllers
{
    public class AdminController : Controller
    {
        private readonly IBowlingAlleyRepository _bowlingAlleyRepository;

        public AdminController(IBowlingAlleyRepository bowlingAlleyRepository)
        {
            _bowlingAlleyRepository = bowlingAlleyRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReservationDetails()
        {
            var reservations = _bowlingAlleyRepository.GetReservationDetails();
            return View(reservations);
        }

        /*public IActionResult Approve(int reservationId)
        {
            _bowlingAlleyRepository.ApproveReservation(reservationId);
            return RedirectToAction("ReservationDetails");
        }

        public IActionResult Reject(int reservationId)
        {
            _bowlingAlleyRepository.RejectReservation(reservationId);
            return RedirectToAction("ReservationDetails");
        }*/


    }
}
