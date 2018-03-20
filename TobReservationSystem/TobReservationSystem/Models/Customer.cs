using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
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

        [Display(Name = "Customer Reference Code")]
        public string CustomerRefCode { get; set; }





        public string GenerateCustomerRefCode()
        {
            var second = DateTime.Now.TimeOfDay.ToString();
            var milliseconds = second.Substring(10);

            Regex rgx = new Regex(@"\s+");
            var nameCode = rgx.Replace(Name, "");

            if (nameCode.Length < 5)
                nameCode = nameCode.Substring(0, nameCode.Length);
            else
                nameCode = nameCode.Substring(0, 5);

            return (nameCode + milliseconds).ToUpper();
        }
    }
}