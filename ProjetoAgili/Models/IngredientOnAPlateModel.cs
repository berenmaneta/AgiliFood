using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class IngredientOnAPlateModel
    {
        public int IdIngredientOnAPlate { get; set; }
        public int IdDish { get; set; }
        public int IdIngredient { get; set; }

        public virtual Dishes Dishes { get; set; }
        public virtual Ingredients Ingredients { get; set; }
    }
}