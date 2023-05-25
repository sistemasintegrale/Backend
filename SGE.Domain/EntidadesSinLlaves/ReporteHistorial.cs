using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.EntidadesSinLlaves
{
    public class ReporteHistorial
    {
        public string Placa { get; set; } = null!;
        public string NombreCliente { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string NumeroOrden { get; set; } = null!;
        public string Situacion { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
        public DateTime FechaOrden { get; set; }
        public string DescripcionTipoServicio { get; set; } = null!;
        public string Kilometraje { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public string DescripcionServicio { get; set; } = null!;
        public decimal PrecioTotalItem { get; set; }
        public int CodigoMoneda { get; set; }
    }
}
