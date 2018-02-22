using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobReservationSystem.Models;
using TobReservationSystem.ViewModels;

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
        public ViewResult Index()
        {
            // gets all coach journeys in the database, toList() executes the query
            var coachJourneys = _context.CoachJourneys.ToList();

            return View(coachJourneys);
        }

        // GET: CoachJourneys/New
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
        public ActionResult Edit(int id)
        {
            // gets the coach journeys details by accessing it's id
            var coachJourneyInDb = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            if (coachJourneyInDb == null)
                return HttpNotFound();

            // gets the list of departing from centers and executes the query
            var departFromCenters = _context.DepartFromCenters.ToList();

            var viewModel = new CoachJourneyFormViewModel
            {
                CoachJourney = coachJourneyInDb,
                DepartFromCenters = departFromCenters
            };
            return View("CoachJourneyForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CoachJourney coachJourney)
        {
            // validation check on the data entered
            if (!ModelState.IsValid) // executes if data fails to meet validation requirements
            {
                var viewModel = new CoachJourneyFormViewModel
                {
                    CoachJourney = coachJourney,
                    DepartFromCenters = _context.DepartFromCenters.ToList()
                };

                return View("CoachJourneyForm", viewModel);
            }
            // a coach journey that does not exist will have a default Id of 0
            if (coachJourney.Id == 0)
                _context.CoachJourneys.Add(coachJourney);

            else
            {
                // updates the properties in the database
                var coachJourneyInDb = _context.CoachJourneys.Single(c => c.Id == coachJourney.Id);

                coachJourneyInDb.Destination = coachJourney.Destination;
                coachJourneyInDb.DateOfJourney = coachJourney.DateOfJourney;
                coachJourneyInDb.SeatsAvailable = coachJourney.SeatsAvailable;
                coachJourneyInDb.DepartFromCenterId = coachJourney.DepartFromCenterId;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /CoachJourneys/Delete/2
        // returns a view of the coach journey, where a HttpPost submit (button) handles the deletion (see Delete.cshtml)
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
        [HttpPost, ActionName("Delete")] // renams the action so routing works
        public ActionResult DeleteConfirmed(int id)
        {
            var coachJourneyInDb = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            _context.CoachJourneys.Remove(coachJourneyInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}