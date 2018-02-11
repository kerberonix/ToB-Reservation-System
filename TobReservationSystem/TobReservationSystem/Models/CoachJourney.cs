using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TobReservationSystem.Models
{
    public class CoachJourney
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Destination { get; set; }

        [Required]
        public byte SeatsAvailable { get; set; }

        [Required]
        public DateTime DateOfJourney { get; set; }
    }
}