﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangyModels
{
    public class OrderHeaderDTO
    {
     
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }

        [Required]
        public string Status { get; set; }   

        [Display(Name ="Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name ="Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name ="Email")]
        [Required]
        public string Email { get; set; }

        public string? PaymentintentId { get; set; }
        public string? SessionId { get; set; }
        public string? Tracking { get; set; }
        public string? Carrier { get; set; }
    }
}
