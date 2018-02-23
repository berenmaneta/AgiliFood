using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class ShowOrdersModel
    {
        public List<OrdersModel> lista { get; set; }
        public double Total { get; set; }
        public string UserName { get; set; }
        public int IdUser { get; set; }
        public int IdMes { get; set; }
        public string Mes { get; set; }
        public List<MesesModel> Meses { get; set; }
    }
}