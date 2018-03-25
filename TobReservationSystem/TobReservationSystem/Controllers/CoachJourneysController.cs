using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobReservationSystem.Models;
using TobReservationSystem.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace TobReservationSystem.Controllers
{
    public class CoachJourneysController : Controller
    {
        // declares a DbContext which allows access to a database
        private ApplicationDbContext _context;

        public CoachJourneysController()
        {
            // initializes DbContext in the constructor
            _context = new ApplicationDbContext();
        }

        // disposes the DbContext
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: CoachJourneys
        public ViewResult Index(string search, int? page)
        {

            if (string.IsNullOrWhiteSpace(search)) // if no search was specified
            {
                // gets all coach journeys in the database, toList() executes the query
                var coachJourneys = _context.CoachJourneys.ToList().ToPagedList(page ?? 1, 5);

                // checks if the user is signed in as an admin
                if (User.IsInRole("CanManageCoachJourneys"))
                    return View("List", coachJourneys);

                return View("ReadOnlyList", coachJourneys); // return the list of all coachJourneys
            }

            var searchResult = _context.CoachJourneys // else a search is conducted
            .Where(x => x.Destination.Contains(search))
            .OrderBy(i => i.Destination)
            .ToPagedList(page ?? 1, 5);

            // checks if the user is signed in as an admin
            if (User.IsInRole("CanManageCoachJourneys"))
                return View("List", searchResult); // return a list of seach items with the admin view

            return View("ReadOnlyList", searchResult); // return a list of seach items with the regular view
        }

        // GET: CoachJourneys/New
        [Authorize(Roles = RoleName.CanManageCoachJourneys)]
        public ViewResult New()
        {
            var departFromCenters = _context.DepartFromCenters.ToList();

            var viewModel = new CoachJourneyFormViewModel
            {
                DepartFromCenters = departFromCenters
            };

            return View("CoachJourneyForm", viewModel);
        }

        // GET: CoachJourneys/Edit/2
        // returns a view of the coachJourney form (see CoachJourneyForm.cshtml)
        [Authorize(Roles = RoleName.CanManageCoachJourneys)]
        public ActionResult Edit(int id)
        {
            // gets the coach journeys details by matching its id to the id passed in the URL
            var coachJourneyInDb = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            if (coachJourneyInDb == null)
                return HttpNotFound();

            // gets the list of departing from centers and executes the query
            var departFromCenters = _context.DepartFromCenters.ToList();

            // mapping the data to the view from the db
            var viewModel = new CoachJourneyFormViewModel
            {
                CoachJourney = coachJourneyInDb,
                DepartFromCenters = departFromCenters
            };
            return View("CoachJourneyForm", viewModel);
        }

        // POST: new record / update existing record
        // the action which peforms the actual POST request to add a new CoachJourney to the database (performed when submit button is pressed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageCoachJourneys)]
        public ActionResult Save(CoachJourney coachJourney)
        {
            // code executed if validation check fails
            if (!ModelState.IsValid)
            {
                // mapping the data to the view from the db (the data is reset to the data in the db if validation failed)
                var viewModel = new CoachJourneyFormViewModel
                {
                    CoachJourney = coachJourney,
                    DepartFromCenters = _context.DepartFromCenters.ToList()
                };

                return View("CoachJourneyForm", viewModel);
            }
            // a coach journey that does not exist will have a default Id of 0
            // adds a new coachJourney (which was created by the POST request) to the db
            if (coachJourney.Id == 0)
            {
                coachJourney.TicketsAvailable = coachJourney.TotalNumberOfTickets;
                _context.CoachJourneys.Add(coachJourney);

            }

            else
            {
                // updates the exiting coachJourney in the db
                // updates the individual properties and overwrites the previous entry in the db
                var coachJourneyInDb = _context.CoachJourneys.Single(c => c.Id == coachJourney.Id);

                coachJourneyInDb.Destination = coachJourney.Destination;
                coachJourneyInDb.DateOfJourney = coachJourney.DateOfJourney;
                coachJourneyInDb.TicketsAvailable -= (coachJourneyInDb.TotalNumberOfTickets - coachJourney.TotalNumberOfTickets);
                coachJourneyInDb.TotalNumberOfTickets = coachJourney.TotalNumberOfTickets;
                coachJourneyInDb.DepartFromCenterId = coachJourney.DepartFromCenterId;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /CoachJourneys/Delete/2
        // returns a view of the coach journey, where a HttpPost request (submit button) on the view handles the actual deletion
        [Authorize(Roles = RoleName.CanManageCoachJourneys)]
        public ActionResult Delete(int id)
        {
            var coachJourneyInDb = _context.CoachJourneys.Include(c => c.DepartFromCenter).SingleOrDefault(c => c.Id == id);

            if (coachJourneyInDb == null)
            {
                return HttpNotFound();
            }
            return View(coachJourneyInDb);
        }

        // POST: /CoachJourneys/Delete/2
        // the action which peforms the actual deletion of the record
        [HttpPost, ActionName("Delete")] // renames the action so routing works
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageCoachJourneys)]
        public ActionResult DeleteConfirmed(int id)
        {
            var coachJourneyInDb = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            _context.CoachJourneys.Remove(coachJourneyInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}