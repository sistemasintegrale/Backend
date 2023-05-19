using Microsoft.AspNetCore.Mvc;
using SGE.Application.Interfaces;
using SGE.Domain.Dtos.Usuario;

namespace SGE.API.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController
    {
        private IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDataDto>> Post([FromBody] UsuarioCreateDto dto)
        {
            var usuario = await _usuarioRepository.Create(dto);
            return usuario;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UsuarioDataDto>> Put([FromRoute] int id, [FromBody] UsuarioEditDto dto)
        {
            var usuario = await _usuarioRepository.Update(dto, id);
            return usuario;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            var response = await _usuarioRepository.Delete(id);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDataDto>>> Get()
        {
            var response = await _usuarioRepository.GetAll();
            return response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UsuarioDataDto>> Get([FromRoute] int id)
        {
            var response = await _usuarioRepository.GetById(id);
            return response;
        }
    }
}
