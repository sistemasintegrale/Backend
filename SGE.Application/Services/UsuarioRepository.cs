using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGE.Application.Interfaces;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.Entities;
using SGE.Infraestructure.Context;
using SGE.Utilities.Codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Services
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private ApplicationDbContext _applicationDbContext;
        private IMapper _mapper;

        public UsuarioRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<UsuarioDataDto> Create(UsuarioCreateDto proveedorDTO)
        {
            var usuarioDB = _mapper.Map<Usuario>(proveedorDTO);
            usuarioDB.Password = Codec.Encriptar(proveedorDTO.Password);
            await _applicationDbContext.Usuarios.AddAsync(usuarioDB);
            await _applicationDbContext.SaveChangesAsync();
            return _mapper.Map<UsuarioDataDto>(usuarioDB);
        }

        public async Task<bool> Delete(int id)
        {
            var usuarioDb = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuarioDb is null)
                return false;
            _applicationDbContext.Usuarios.Remove(usuarioDb);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<UsuarioDataDto>> GetAll()
        {
            var usuariosDB = await _applicationDbContext.Usuarios.ToListAsync();
            return _mapper.Map<List<UsuarioDataDto>>(usuariosDB);
        }

        public async Task<UsuarioDataDto> GetById(int id)
        {
            var usuarioDB = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<UsuarioDataDto>(usuarioDB);
        }

        public async Task<UsuarioDataDto> Update(UsuarioEditDto usuarioDto, int id)
        {
            var usuarioDB = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x=>x.Id == id);
            usuarioDB.Nombre = usuarioDto.Nombre;
            usuarioDB.Apellidos = usuarioDto.Apellidos;
            usuarioDB.Email = usuarioDto.Email;
            usuarioDB.Password = usuarioDto.Password;
            usuarioDB.Estado = usuarioDto.Estado;
            _applicationDbContext.Usuarios.Update(usuarioDB);
            await _applicationDbContext.SaveChangesAsync();
            return _mapper.Map<UsuarioDataDto>(usuarioDB);
        }
    }
}
