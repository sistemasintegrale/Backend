using System;
using AutoMapper;
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
        }
	}
}

