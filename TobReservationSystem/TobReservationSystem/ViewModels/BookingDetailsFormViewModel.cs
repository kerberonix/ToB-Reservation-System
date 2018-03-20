using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TobReservationSystem.Models;

namespace TobReservationSystem.ViewModels
{
    public class BookingDetailsFormViewModel
    {
        // Id doesn't need to be mapped, the Id's purpose is to retrieve the Id of a booking when updating or deleting a database entry
        public int Id { get; set; }

        public string Destination { get; set; }

        [Display(Name = "Depart From")]
        public string DepartFromCenter { get; set; }

        [Display(Name = "Date of Journey")]
        public DateTime DateOfJourney { get; set; }

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer Reference Code")]
        public string CustomerRefCode { get; set; }

        // this validation rule seems unable to be retrieved from the model so im setting it in the view model
        [Range(1, 10)]
        [Display(Name = "Number of Tickets")]
        public int TicketQuantity { get; set; }

        [Display(Name = "Date of Booking")]
        public DateTime DateOfBooking { get; set; }

        [Display(Name = "Tickets Available")]
        public int TicketsAvailable { get; set; }


        // *Managing ticket availability*
        // this method updates the number of available tickets when an existing booking's ticket quantity is changed
        public int UpdateAvailableTickets(Booking bookingInDb)
        {
            // if some tickets are refunded
           if (bookingInDb.TicketQuantity > TicketQuantity)
                 bookingInDb.CoachJourney.TicketsAvailable += (bookingInDb.TicketQuantity - TicketQuantity);

            // if more tickets are bought
            else if (bookingInDb.TicketQuantity < TicketQuantity)
                 bookingInDb.CoachJourney.TicketsAvailable -= (TicketQuantity - bookingInDb.TicketQuantity);

           // checks if more tickets are bought that are available
           if (bookingInDb.CoachJourney.TicketsAvailable < 0)
                return bookingInDb.CoachJourney.TicketsAvailable;

            return bookingInDb.TicketQuantity = TicketQuantity;
        }
    }
}