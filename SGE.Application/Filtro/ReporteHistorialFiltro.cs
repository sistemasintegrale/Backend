﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Filtro
{
    public class ReporteHistorialFiltro
    {
        public string fechaDesde { get; set; } = null!;
        public string fechaHasta { get; set; } = null!;
        public int desde { get; set; }
        public int hasta { get; set;}
        public int marca { get; set; }
        public int modelo { get; set; }
        public int placa { get; set; }
        public int orden { get; set; }
    }
}
