using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TobReservationSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Display(Name = "Date of Booking")]
        public DateTime DateOfBooking { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "Ticket Quantity")]
        public int TicketQuantity { get; set; }

        [Required]
        public Customer Customer { get; set; }  // navigation property

        public int CustomerId { get; set; }

        [Required]
        public CoachJourney CoachJourney { get; set; }  // navigation property

        public int CoachJourneyId { get; set; }

        public int ReturnTickets()
        {
            return CoachJourney.TicketsAvailable += TicketQuantity;
        }
    }
}