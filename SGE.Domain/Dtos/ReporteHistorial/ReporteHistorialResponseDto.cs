﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Domain.Dtos.ReporteHistorial
{
    public class ReporteHistorialResponseDto
    {
        public string Placa { get; set; } = null!;
        public string NombreCliente { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public string NumeroOrden { get; set; } = null!;
        public string Situacion { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;
        public string FechaOrden { get; set; } = null!;
        public string DescripcionTipoServicio { get; set; } = null!;
        public string Kilometraje { get; set; } = null!;
        public decimal Cantidad { get; set; } 
        public string DescripcionServicio { get; set; } = null!;
        public decimal PrecioTotalItem { get; set; }  
        public string CodigoMoneda { get; set; } = null!;
    }
}