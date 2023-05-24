using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Pagination;
using SGE.Domain.Dtos.Token;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<PaginationResponse<BaseResponse<List<UsuarioDataDto>>>> GetAll(UsuarioFilter filters);

        public Task<BaseResponse<UsuarioDataDto>> GetById(int id);
        public Task<BaseResponse<UsuarioDataDto>> GetByIdPass(int id);

        public Task<BaseResponse<UsuarioDataDto>> Create(UsuarioCreateDto proveedorDTO);

        public Task<BaseResponse<UsuarioDataDto>> Update(UsuarioEditDto proveedorDTO, int id);

        public Task<BaseResponse<bool>> Delete(int id);

        public Task<BaseResponse<string>> GenerateToken(TokenRequestDto tokenRequestDto);

        public Task<Usuario> AccountByuserName(string userName);

        public Task<BaseResponse<UsuarioDataDto>> AccountByuserEmail(string userName);

    }
}
