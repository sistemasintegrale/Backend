using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Utilities.Enum;
using System.Security.Claims;

namespace SGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrdenReparacionController : ControllerBase
    {
        private readonly IOrdenReparacionRepository _reportesRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public OrdenReparacionController(IOrdenReparacionRepository reportesRepository, IUsuarioRepository usuarioRepository)
        {
            _reportesRepository = reportesRepository;
            _usuarioRepository = usuarioRepository;

        }

        [HttpPost("{service:int}")]
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<OrdenReparacionList>>>>> Post([FromBody] ReporteHistorialFiltro filtro, [FromRoute] int service)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.OrdenReparacion(filtro, service, icod);
        }
    }
}
