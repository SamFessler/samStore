﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samStore.Models
{
    public class OrderUserModel
    {
        public OrderModel Order { get; set; }
        public int? OrderQuantity { get; set; }
    }
}