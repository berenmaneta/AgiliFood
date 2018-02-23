using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class RestaurantsModel
    {
        public int IdRestaurant { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string Email { get; set; }
        public int Enabled { get; set; }
    }
}