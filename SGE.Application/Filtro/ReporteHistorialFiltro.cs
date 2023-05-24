using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Filtro
{
    public class ReporteHistorialFiltro
    {
        public DateTime fechaDesde { get; set; }
        public DateTime fechaHasta { get; set; }
        public int desde { get; set; }
        public int hasta { get; set;}
    }
}
