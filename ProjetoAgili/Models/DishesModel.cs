using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class DishesModel
    {
        public int IdDish { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public int IdRestaurant { get; set; }
        public double Price { get; set; }
        public byte[] Photo { get; set; }
        public int IsOnMenu { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string RestaurantName { get; set; }
        public int IdCategory { get; set; }
        public List<IngredientesModel> Ingredients { get; set; }
        public List<CategoryModel> Categorias { get; set; }
    }
}