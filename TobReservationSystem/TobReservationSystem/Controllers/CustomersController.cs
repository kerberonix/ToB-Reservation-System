using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TobReservationSystem.Models;

namespace TobReservationSystem.Controllers
{
    public class CustomersController : Controller
    {

        // declares a DbContext which allows access to a database
        private ApplicationDbContext _context;

        public CustomersController()
        {
            // initializes DbContext in the constructor
            _context = new ApplicationDbContext();
        }

        // disposes the DbContext
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ViewResult Index()
        {
            // gets all customers in the database, toList() executes the query
            var customers = _context.Customers.ToList();

            return View(customers);
        }

        // GET: Customers/Details/id
        public ActionResult Details(int id)
        {
            // gets customers in the database and executes the query
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }
    }
}