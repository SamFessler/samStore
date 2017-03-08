using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace samStore.Models
{
    public class CheckoutModel
    {

        [Display(Name = "Shipping Address Line 1")]
        [Required(ErrorMessage ="Please input adress to continue")]
        public string ShippingAddress1 { get; set; }

        [Display(Name = "Shipping Address Line 2")]
        public string ShippingAddress2 { get; set; }

        [Display(Name = "Shipping City")]
        [Required(ErrorMessage = "Please input adress to continue")]
        public string ShippingCity { get; set; }

        //[]
        //public string ShippingCountry { get; set; }

        [Display(Name ="Shipping State")]
        [Required(ErrorMessage = "Please input state to continue")]
        public string ShippingState { get; set; }

        [Display(Name ="Shipping Zip Code")]
        [Required(ErrorMessage = "Please input your Zip  Code to continue")]
        public string ShippingZip { get; set; }

    }
}