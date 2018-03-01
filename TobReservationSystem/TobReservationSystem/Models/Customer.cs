﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TobReservationSystem.Models
{
    public class Customer
    {
        [Display (Name = "Customer Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public MembershipType MembershipType { get; set; } // navigation property

        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }
    }
}