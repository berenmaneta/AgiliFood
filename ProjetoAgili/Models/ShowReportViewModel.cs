using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoAgili.Models
{
    public class ShowReportViewModel
    {
        public List<ReportModel> Reports { get; set; }
        public double Total { get; set; }
        public int IdMes { get; set; }
    }
}