using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samStore.Models
{
    public class State
    {
        public int ID { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        
    }
}