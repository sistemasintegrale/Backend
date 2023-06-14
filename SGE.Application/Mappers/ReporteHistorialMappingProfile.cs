using System;
using AutoMapper;
using SGE.Domain.Dtos;
using SGE.Domain.Dtos.Marca;
using SGE.Domain.Dtos.Modelo;
using SGE.Domain.Dtos.OrdenReparacion;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Domain.Entities;

namespace SGE.Application.Mappers
{
	public class ReporteHistorialMappingProfile : Profile
    {
		public ReporteHistorialMappingProfile()
		{
            CreateMap<ReporteHistorialResponseDto, ReporteHistorial>().ReverseMap();
            CreateMap<MarcaDto, Marca>().ReverseMap();
            CreateMap<ModeloDto, Modelo>().ReverseMap();
            CreateMap<PlacaDto, Placa>().ReverseMap();
            CreateMap<OrdenReparacionDto, OrdenReparacion>().ReverseMap();
            CreateMap<ClienteDto, Cliente>().ReverseMap();
        }
	}
}

