using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class OrdersModel
    {
        public int IdPedido { get; set; }
        public int IdUser { get; set; }
        public int IdDish { get; set; }
        public System.DateTime Date { get; set; }
        public double Price { get; set; }
        public string DishName { get; set; }
        public string RestaurantName { get; set; }
        public string UserName { get; set; }
        public string Mes { get; set; }
        public int IdMes { get; set; }
    }
}