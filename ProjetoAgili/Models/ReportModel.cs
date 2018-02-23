using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class ReportModel
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public double Total { get; set; }
        public List<PedidoModel> Pedidos { get; set; }
    }
}