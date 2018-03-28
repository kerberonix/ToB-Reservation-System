using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TobReservationSystem.Models;
using TobReservationSystem.ViewModels;
using PagedList;
using PagedList.Mvc;

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

        // GET: /Bookings
        public ViewResult Index(string search, int? page)
        {
            if (string.IsNullOrWhiteSpace(search))  // if no search was specified
            {
                var bookings = _context.Bookings
                    .Include(c => c.Customer)
                    .Include(c => c.CoachJourney)
                    .ToList()
                    .ToPagedList(page ?? 1, 5);

                return View(bookings); // return the list of all bookings
            }

            var searchResult = _context.Bookings // else a search is conducted
                .Include(c => c.Customer)
                .Include(c => c.CoachJourney)
                .Where(x => x.Customer.CustomerRefCode.Contains(search))
                .OrderBy(i => i.Customer.CustomerRefCode)
                .ToPagedList(page ?? 1, 5);

            return View(searchResult); // return a list of seach items
        }

        // GET: /Bookings/EditBooking/1
        // returns a view of the booking details form (see BookingDetailsForm.cshtml)
        public ActionResult EditBooking(int id)
        {
            var booking = _context.Bookings
                .Include("CoachJourney.DepartFromCenter")
                .Include(c => c.Customer)
                .SingleOrDefault(c => c.Id == id);

            if (booking == null)
                return HttpNotFound();

            // mapping the data to the view from the db
            var viewModel = new BookingDetailsFormViewModel
            {
                Destination = booking.CoachJourney.Destination,
                DepartFromCenter = booking.CoachJourney.DepartFromCenter.Name,
                DateOfJourney = booking.CoachJourney.DateOfJourney,
                CustomerName = booking.Customer.Name,
                TicketQuantity = booking.TicketQuantity,
                DateOfBooking = booking.DateOfBooking,
                TicketsAvailable = booking.CoachJourney.TicketsAvailable,
                CustomerRefCode = booking.Customer.CustomerRefCode
            };

            return View("BookingDetailsForm", viewModel);
        }

        // POST: /Bookings/SaveBooking/1
        // the action which peforms the actual POST request to update an exisitng booking in the database (performed when submit button is pressed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBooking(BookingDetailsFormViewModel bookingDetails)
        {

            var bookingInDb = _context.Bookings
                 .Include("CoachJourney.DepartFromCenter")
                 .Include(c => c.Customer)
                 .Single(c => c.Id == bookingDetails.Id);

            // code executed if validation check fails
            if (!ModelState.IsValid)
            {
                // resets the data on the view
                // mapping the data to the view from the db (the data is reset to the data in the db if validation failed)
                var viewModel = new BookingDetailsFormViewModel
                {
                    Destination = bookingInDb.CoachJourney.Destination,
                    DepartFromCenter = bookingInDb.CoachJourney.DepartFromCenter.Name,
                    DateOfJourney = bookingInDb.CoachJourney.DateOfJourney,
                    CustomerName = bookingInDb.Customer.Name,
                    TicketQuantity = bookingInDb.TicketQuantity,
                    DateOfBooking = bookingInDb.DateOfBooking,
                    TicketsAvailable = bookingInDb.CoachJourney.TicketsAvailable,
                    CustomerRefCode = bookingInDb.Customer.CustomerRefCode
                };

                return View("BookingDetailsForm", viewModel);
            }

            else // executed if validation check passes
            {
                // *Code block to manage ticket availability*
                // updates the number of tickets on sale in Coachjourneys
                bookingInDb.TicketQuantity = bookingDetails.UpdateAvailableTickets(bookingInDb);

                if (bookingInDb.TicketQuantity < 0)
                    throw new Exception("not enough tickets available");
                // saving data to the db
                _context.SaveChanges();

            }

            return RedirectToAction("Index", "Bookings");
        }

        // POST: /Bookings/DeleteBooking/1
        // the action which peforms the actual POST request to delete an exisitng booking in the database (performed when submit button is pressed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBooking(int id)
        {
            // get the booking in the db
            var booking = _context.Bookings.Single(c => c.Id == id);

            // return the tickets to the stock
            var coachJourney = _context.CoachJourneys.Single(c => c.Id == booking.CoachJourneyId);
            booking.ReturnTickets();

            // remove it from the db
            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return RedirectToAction("Index", "Bookings");
        }

        // GET: /Bookings/NewBooking/1
        // returns a view of the booking form (see BookingForm.cshtml)
        public ActionResult NewBooking(int id)
        {
            // gets the CoachJourney from the db that a booking is being made to
            var coachJourneyInDb = _context.CoachJourneys
                .Include(c => c.DepartFromCenter)
                .SingleOrDefault(c => c.Id == id);

            if (coachJourneyInDb == null)
                return HttpNotFound();

            // mapping the data to the view from the db (to make a new booking, dat about theCochJourney for that booking needs to be retrieved from the db)
            // We don't need to pass the customerId in the viewModel here, as we have not specified what customer we want to make a booking for yet
            var viewModel = new BookingFormViewModel
            {
                CoachJourneyId = coachJourneyInDb.Id,
                Destination = coachJourneyInDb.Destination,
                TicketQuantity = 1,
                TicketsAvailable = coachJourneyInDb.TicketsAvailable,
                DepartFromCenter = coachJourneyInDb.DepartFromCenter.Name
            };

            return View("BookingForm", viewModel);
        }

        // POST: /Bookings/SaveNewBooking/1
        // the action which peforms the actual POST request to add a new booking to the database (performed when submit button is pressed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveNewBooking(BookingFormViewModel newBooking)
        {
            // matches the coachJourneyId to the coachJourneyId stored in the booking table to get the correct coachJourney details (this was mapped in the New action above)
            var coachJourney = _context.CoachJourneys.Single(c => c.Id == newBooking.CoachJourneyId);

            // matches the customer reference code provided in the POST request (in the input text box) to the corresponding customer stored in the Db
            var customer = _context.Customers.SingleOrDefault(c => c.CustomerRefCode == newBooking.CustomerRefCode);

            // code executed if validation check fails
            if (!ModelState.IsValid)
            {
                // resets the data on the view
                // mapping the data to the view from the db (the data is reset to the data in the db if validation failed)
                var viewModel = new BookingFormViewModel
                {
                    CoachJourneyId = coachJourney.Id,
                    CustomerId = customer.Id,
                    Destination = coachJourney.Destination,
                    TicketQuantity = 1,
                    CustomerRefCode = customer.CustomerRefCode
                };

                return View("BookingForm", viewModel);
            }

            if (customer == null)
                return new HttpStatusCodeResult(404, "Customer Id Not Found");

            else
            {
                // *Code block to manage ticket availability*
                // when there are no tickets left
                if (coachJourney.TicketsAvailable == 0)
                    throw new Exception("no tickets available");

                // when more tickets are attempted to be bought than are available
                else if (coachJourney.TicketsAvailable < newBooking.TicketQuantity)
                    throw new Exception("not enough tickets available");

                // deduct number of tickets bought from number of tickets available
                newBooking.DeductTickets(coachJourney);

                // if everything was successful, add the booking to the Db
                // mapping the model obtained in the POST request to the db properties
                var booking = new Booking
                {
                    CustomerId = customer.Id, // saves the customerId in the Db NOT the ref code (Id should not be visible on the view)
                    CoachJourneyId = newBooking.CoachJourneyId,
                    DateOfBooking = DateTime.Now,
                    TicketQuantity = newBooking.TicketQuantity
                };

                // add the booking containing the mapped model from the POST request to the db
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                return RedirectToAction("Index", "CoachJourneys");
            }

        }
    }
}