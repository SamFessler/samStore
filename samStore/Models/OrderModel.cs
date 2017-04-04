﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samStore.Models
{
    public class OrderModel
    {
        public int? Id { get; set; }
        public int NumberOrdered {get;set;}

        public string OrderNumber { get; set; }
        public string ShippingAddress { get; set; }
        public IEnumerable<OrderProduct> Products { get; set; }
       
        public string EmailUsed { get; set; }

        public decimal? SubTotal { get; set; }



    }
}