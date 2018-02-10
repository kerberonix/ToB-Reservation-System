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
        // GET: Customers
        public ViewResult Index()
        {
            var customers = GetCustomers();

            return View(customers);
        }

        // GET: Customers/Details/id
        public ActionResult Details(int id)
        {
            var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

                return View(customer);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var customers = new List<Customer>

            {
                new Customer { Id = 1, Name = "James Smith", Birthdate = new DateTime(1989, 3, 18) },
                new Customer { Id = 2, Name = "Harry Anderson", Birthdate = new DateTime(1991, 11, 29) }
            };

            return customers;
        }
    }
}