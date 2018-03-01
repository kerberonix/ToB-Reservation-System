using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobReservationSystem.Models;
using TobReservationSystem.ViewModels;

namespace TobReservationSystem.Controllers
{
    public class BookingsController : Controller
    {
        // declares a DbContext which allows access to a database
        private ApplicationDbContext _context;

        public BookingsController()
        {
            // initializes DbContext in the constructor
            _context = new ApplicationDbContext();
        }

        // disposes the DbContext
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: /Bookings/CreateNewBooking/1
        // returns a view of the booking form, where a HttpPost submit (button) POSTS the data to the database (see BookingForm.cshtml)
        public ActionResult CreateNewBooking(int id)
        {
            // gets the journey from the Db that a booking is being made to
            var coachJourneyInDb = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            if (coachJourneyInDb == null)
                return HttpNotFound();

            // We don't need to pass the customer in the viewModel here, as we have not specified what customer we want to make a booking for yet
            var viewModel = new BookingFormViewModel
            {
                CoachJourney = coachJourneyInDb
            };

            return View("BookingForm", viewModel);  
        }

        // POST: /Bookings/CreateNewBooking/1
        // the action which peforms the actual POST request to add a new booking to the database
        [HttpPost, ActionName("CreateNewBooking")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking(Booking newBooking)
        {
            // matches the coachJourney a customer wants to make a booking for to the corresponding coachJourney stored in the Db
            var coachJourney = _context.CoachJourneys.Single(c => c.Id == newBooking.Id);

            // matches the customer Id provided in the POST request (in the input text box) to the corresponding customer stored in the Db
            var customer = _context.Customers.Single(c => c.Id == newBooking.Customer.Id);

            // *Code block to manage ticket availability*
            // when there are no tickets left
            if (coachJourney.TicketsAvailable == 0)
                return Content("There are no tickets left.");

            // when more tickets are attempted to be bought than are available
            else if (coachJourney.TicketsAvailable < newBooking.CoachJourney.TicketsAvailable)
                return Content("There are only " + coachJourney.TicketsAvailable + " tickets left, buy less.");

            // deduct number of tickets bought from number of tickets available
            coachJourney.TicketsAvailable -= newBooking.CoachJourney.TicketsAvailable;

            var booking = new Booking
            {
                Customer = customer,
                CoachJourney = coachJourney,
                DateOfBooking = DateTime.Now,
                TicketQuantity = newBooking.CoachJourney.TicketsAvailable
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Index", "CoachJourneys");
        }
    }
}