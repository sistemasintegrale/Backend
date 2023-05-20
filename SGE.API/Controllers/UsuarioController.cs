using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Bases;
using SGE.Application.Interfaces;
using SGE.Domain.Dtos.Token;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.Entities;
using SGE.Utilities.Codec;

namespace SGE.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/usuario")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<UsuarioDataDto>>> Post([FromBody] UsuarioCreateDto dto)
        {
            var usuario = await _usuarioRepository.Create(dto);
            return usuario;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BaseResponse<UsuarioDataDto>>> Put([FromRoute] int id, [FromBody] UsuarioEditDto dto)
        {
            var usuario = await _usuarioRepository.Update(dto, id);
            return usuario;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BaseResponse<bool>>> Delete([FromRoute] int id)
        {
            var response = await _usuarioRepository.Delete(id);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<UsuarioDataDto>>>> Get()
        {

            var response = await _usuarioRepository.GetAll();
            return response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BaseResponse<UsuarioDataDto>>> Get([FromRoute] int id)
        {
            var response = await _usuarioRepository.GetById(id);
            return response;
        }

        [AllowAnonymous]
        [HttpPost("generate/token")]
        public async Task<ActionResult<BaseResponse<string>>> Post([FromBody] TokenRequestDto tokenRequestDto)
        {
            var response = await _usuarioRepository.GenerateToken(tokenRequestDto);
            return response;
        }

        [HttpGet("validarToken")]
        public async Task<ActionResult<BaseResponse<UsuarioDataDto>>> GetUserByToken()
        {

            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            BaseResponse<string> nuevoToken = await _usuarioRepository.GenerateToken(new TokenRequestDto() { email = response.Data.Email, password = Codec.DesEncriptar(response.Data.Password) });
            response.Data.Token = nuevoToken.Data;
            return response;
        }
    }
}
