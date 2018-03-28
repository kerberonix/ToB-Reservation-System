using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TobReservationSystem.Models;

namespace TobReservationSystem.ViewModels
{
    public class BookingFormViewModel
    {
        public int CoachJourneyId { get; set; }

        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        public string Destination { get; set; }

        // this validation rule seems unable to be retrieved from the model so im setting it in the view model
        [Range(1, 10)]
        [Display(Name = "Ticket Quantity")]
        public int TicketQuantity { get; set; }

        [Display(Name = "Tickets Available")]
        public int TicketsAvailable { get; set; }

        [Display(Name = "Departing From")]
        public string DepartFromCenter { get; set; }

        [Display(Name = "Customer Reference Code")]
        public string CustomerRefCode { get; set; }


        public int DeductTickets(CoachJourney coachJourney)
        {
            return coachJourney.TicketsAvailable -= TicketQuantity;
        }
    }
}