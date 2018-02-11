using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobReservationSystem.Models;

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

        // GET: CoachJourneys/Details/id
        public ActionResult Details(int id)
        {
            // gets coach journeys in the database and executes the query
            var coachJourney = _context.CoachJourneys.SingleOrDefault(c => c.Id == id);

            if (coachJourney == null)
                return HttpNotFound();

            return View(coachJourney);
        }
    }
}