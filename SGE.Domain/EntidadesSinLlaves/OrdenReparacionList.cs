using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.EntidadesSinLlaves
{
    public class OrdenReparacionList
    {
        public string NumeroOrden { get; set; } = null!;
        public string NumeroPresupuesto { get; set; } = null!;
        public string FechaOrden { get; set; } = null!;
        public string Cliente { get; set; } = null!;
        public string CiaSeguro { get; set; } = null!;
        public string Placa { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string Motor { get; set; } = null!;
        public int Anio { get; set; }
        public string Situacion { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
        public string FechaDocumento { get; set; } = null!;
        public string OrdenCompra { get; set; } = null!;
        public decimal Importe { get; set; }
        public string Moneda { get; set; } = null!;
    }
     
}
