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

        public DateTime DateOfBooking { get; set; }

        public int TicketQuantity { get; set; }

        public int CustomerId { get; set; }
        public int CoachJourneyId { get; set; }


        // navigation properties
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public CoachJourney CoachJourney { get; set; }
    }
}