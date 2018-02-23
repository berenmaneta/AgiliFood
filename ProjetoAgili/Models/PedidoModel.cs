using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class PedidoModel
    {
        public int IdPedido { get; set; }
        public int IdUser { get; set; }
        public int IdDish { get; set; }
        public System.DateTime Date { get; set; }
        public double Price { get; set; }

        public virtual Dishes Dishes { get; set; }
        public virtual Users Users { get; set; }
    }
}