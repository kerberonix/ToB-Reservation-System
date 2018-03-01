using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TobReservationSystem.Models;

namespace TobReservationSystem.ViewModels
{
    public class BookingFormViewModel
    {
        public CoachJourney CoachJourney { get; set; }
        public Customer Customer { get; set; }
    }
}