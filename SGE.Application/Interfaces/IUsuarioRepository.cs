using SGE.Domain.Dtos.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<List<UsuarioDataDto>> GetAll();

        public Task<UsuarioDataDto> GetById(int id);

        public Task<UsuarioDataDto> Create(UsuarioCreateDto proveedorDTO);

        public Task<UsuarioDataDto> Update(UsuarioEditDto proveedorDTO, int id);

        public Task<bool> Delete(int id);
    }
}
