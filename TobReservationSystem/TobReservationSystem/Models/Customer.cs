﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TobReservationSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public MembershipType MembershipType { get; set; } // navigation property
        public byte MembershipTypeId { get; set; }
    }
}