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
        [Display(Name = "Seats Available")]
        [Range(1, 50)]
        public byte SeatsAvailable { get; set; }

        [Required]
        [Display(Name = "Date of Journey")]
        public DateTime DateOfJourney { get; set; }

        public DepartFromCenter DepartFromCenter { get; set; } // navigation property

        [Display(Name = "Departing From")]
        public byte DepartFromCenterId { get; set; }

    }
}