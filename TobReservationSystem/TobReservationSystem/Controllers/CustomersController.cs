using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TobReservationSystem.Models;
using TobReservationSystem.ViewModels;

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
            var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            return View(customers);
        }

        // GET: Customers/New
        public ViewResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        // GET: Customers/Edit/2
        public ActionResult Edit(int id)
        {
            // gets the customer details by accessing it's id
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return HttpNotFound();

            // gets the list of membership types and executes the query
            var membershipTypes = _context.MembershipTypes.ToList();

            var viewModel = new CustomerFormViewModel
            {
                Customer = customerInDb,
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Customer customer)
        {
            // a customer that does not exist will have a default Id of 0
            if (customer.Id == 0)
                _context.Customers.Add(customer);

            else
            {
                // updates the properties in the database
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
            }

            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        // GET: /Customers/Delete/2
        // returns a view of the customer, where a HttpPost submit (button) handles the deletion (see Delete.cshtml)
        public ActionResult Delete(int id)
        {
            var customerInDb = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return HttpNotFound();

            return View(customerInDb);
        }
        // POST: /Customers/Delete/2
        // the action which peforms the actual deletion of the record
        [HttpPost, ActionName("Delete")] // renames the action so routing works, because the name of the action the form is attached to is the Delete action "Delete"
        public ActionResult DeleteConfirmed(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}