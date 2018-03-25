using System;
using System.Collections.Generic;
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
        public ViewResult Index(string search, int? page)
        {
            if (string.IsNullOrWhiteSpace(search)) // if no search was specified
            {
                // dislay all customers
                // gets all customers in the database, toList() executes the query, toPagedList returns a paged list of all the customers executed by the query
                var customers = _context.Customers.Include(c => c.MembershipType).ToList().ToPagedList(page ?? 1, 5);
                return View(customers); // return the list of all customers
            }

            var searchResult = _context.Customers // else a search is conducted
                .Include(c => c.MembershipType)
                .Where(x => x.Name.Contains(search))
                .OrderBy(i => i.Name) // ToPagedList requires an OrderBy in the expression
                .ToPagedList(page ?? 1, 5); // page ?? 1: if page has value of null, use value of 1, else use value stored in page
                                            // 5: the number of records to display on the page

            return View(searchResult);  // return a list of seach items
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
        // returns a view of the customers form (see CustomerForm.cshtml)
        public ActionResult Edit(int id)
        {
            // gets the customers details by matching its id to the id passed in the URL
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return HttpNotFound();

            // gets the list of membership types and executes the query
            var membershipTypes = _context.MembershipTypes.ToList();

            // mapping the data to the view from the db
            var viewModel = new CustomerFormViewModel
            {
                Customer = customerInDb,
                MembershipTypes = membershipTypes
            };

            return View("CustomerForm", viewModel);
        }
        // POST: new record / update existing record
        // the action which peforms the actual POST request to add a new customer to the database (performed when submit button is pressed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            // code executed if validation check fails
            if (!ModelState.IsValid) // executes if data fails to meet validation requirements
            {
                // mapping the data to the view from the db (the data is reset to the data in the db if validation failed)
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }

            // a customer that does not exist will have a default Id of 0
            // adds a new customer (which was created by the POST request) to the db
            if (customer.Id == 0)
            {
                customer.CustomerRefCode = customer.GenerateCustomerRefCode();
                _context.Customers.Add(customer);
            }

            else
            {
                // updates the exiting customer in the db
                // updates the individual properties and overwrites the previous entry in the db
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
            }

            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        // GET: /Customers/Delete/2
        // returns a view of the customer, where a HttpPost request (submit button) on the view handles the actual deletion
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}