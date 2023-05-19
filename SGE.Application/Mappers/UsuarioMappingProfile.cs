using AutoMapper;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Mappers
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<UsuarioCreateDto, Usuario>().ReverseMap();
            CreateMap<UsuarioDataDto, Usuario>().ReverseMap();
            CreateMap<UsuarioEditDto, Usuario>().ReverseMap();
        }
    }
}
