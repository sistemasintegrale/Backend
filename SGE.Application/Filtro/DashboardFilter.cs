using System;
namespace SGE.Application.Filtro
{
    public class DashboardFilter
    {
        public int Service { get; set; }
        public string FechaInicio { get; set; } = null!;
        public string FechaFinal { get; set; } = null!;
    }
}

