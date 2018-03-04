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

        [Required]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        public string Destination { get; set; }

        [Required]
        [Range(1, 10)]
        [Display(Name = "Ticket Quantity")]
        public int TicketQuantity { get; set; }
    }
}