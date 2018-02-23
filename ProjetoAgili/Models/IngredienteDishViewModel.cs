using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class IngredienteDishViewModel
    {
        public IngredientOnAPlate ingredientDish { get; set; }

        public List<int> SelectedIngredients { get; set; }

        public int IdIngredient { get; set; }

        public int IdDish { get; set; }

        public virtual List<Ingredients> Ingredients { get; set; }
        public virtual List<Dishes> Dishes { get; set; }

        public IngredienteDishViewModel()
        {

        }

        public IngredienteDishViewModel(IngredientOnAPlate _ingredientDish, List<Ingredients> _Ingredients, List<Dishes> _Dishes)
        {
            ingredientDish = _ingredientDish;
            Ingredients = _Ingredients;
            Dishes = _Dishes;
            SelectedIngredients = new List<int>();
        }
    }
}