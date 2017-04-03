using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace samStore.Models
{
    public class CheckoutModel
    {
        //email
        [EmailAddress]
        public string EmailAddress { get; set; }


        //credit card
        [Required]
        public DateTime? CreditCardExperation { get; set; }
        [Required]
        [CreditCard]
        public string CreditCardNumber { get; set; }
        [Required]
        public string CreditCardName { get; set; }
        [Required]
        public string CardVerificationValue { get; set; }


        //shipping
        [Display(Name = "Shipping Address Line 1")]
        [Required(ErrorMessage ="Please input address to continue")]
        public string ShippingAddress1 { get; set; }

        [Display(Name = "Shipping Address Line 2")]
        public string ShippingAddress2 { get; set; }

        [Display(Name = "Shipping City")]
        [Required(ErrorMessage = "Please input address to continue")]
        public string ShippingCity { get; set; }

        //[]
        //public string ShippingCountry { get; set; }

        [Display(Name ="Shipping State")]
        [Required(ErrorMessage = "Please input state to continue")]
        public string ShippingState { get; set; }

        [Display(Name ="Shipping Zip Code")]
        [MinLength(5)]
        [MaxLength(12)]
        [Required(ErrorMessage = "Please input your Zip-Code to continue")]
        public string ShippingZip { get; set; }


        public string tempLocation { get; set; }
        public string tempOrder { get; set; }

    }
}