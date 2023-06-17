using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.EntidadesSinLlaves
{
    public class ReporteHistorial
    {
        public string? Placa { get; set; } 
        public string? NombreCliente { get; set; } 
        public string? Marca { get; set; } 
        public string? Modelo { get; set; } 
        public string? NumeroOrden { get; set; } 
        public string? Situacion { get; set; } 
        public string? NumeroDocumento { get; set; } 
        public string? FechaOrden { get; set; } 
        public string? DescripcionTipoServicio { get; set; } 
        public string? Kilometraje { get; set; } 
        public decimal Cantidad { get; set; }
        public string? DescripcionServicio { get; set; } 
        public decimal PrecioTotalItem { get; set; }
        public int CodigoMoneda { get; set; }
    }
}
