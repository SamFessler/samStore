using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace samStore.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string TreeName { get; set; }
        public decimal? TreePrice { get; set; }

        public string TreeSpecies { get; set; }
        public string TreeType { get; set;}
        public string TreeDescription { get; set; }

        public IEnumerable<string> TreeImage { get; set; }





    }
}