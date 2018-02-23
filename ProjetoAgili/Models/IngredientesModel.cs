using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class IngredientesModel
    {
        public int IdIngrediente { get; set; }
        public int IdRestaurant { get; set; }
        public string Name { get; set; }
    }
}