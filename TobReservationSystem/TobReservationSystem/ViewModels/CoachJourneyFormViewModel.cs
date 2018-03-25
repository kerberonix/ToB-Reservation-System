using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TobReservationSystem.Models;

namespace TobReservationSystem.ViewModels
{
    public class CoachJourneyFormViewModel
    {
        public CoachJourney CoachJourney { get; set; }
        public IEnumerable<DepartFromCenter> DepartFromCenters { get; set; }

        public string Title
        {
            get
            {
                if (CoachJourney != null && CoachJourney.Id != 0)
                    return "Edit Journey";

                return "New Journey";
            }
        }
    }
}