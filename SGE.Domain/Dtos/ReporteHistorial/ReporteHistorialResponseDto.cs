using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Dtos.ReporteHistorial
{
    public class ReporteHistorialResponseDto
    {
        public string? Placa { get; set; }
        public string? NombreCliente { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? NumeroOrden { get; set; }
        public string? Situacion { get; set; }
        public string? NumeroDocumento { get; set; }
        public DateTime? FechaOrden { get; set; }
        public string? DescripcionTipoServicio { get; set; }
        public string? Kilometraje { get; set; }
        public string? Cantidad { get; set; }
        public string? DescipcionServicio { get; set; }
        public string? PrecioTotalItem { get; set; }
        public string? CodigoMoneda { get; set; }
    }
}
