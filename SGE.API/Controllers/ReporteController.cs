using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Application.Services;
using SGE.Domain.Dtos;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Utilities.Enum;
using System.Security.Claims;

namespace SGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReporteController : ControllerBase
    {
        private readonly IReportesRepository _reportesRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReporteController(IReportesRepository reportesRepository, IUsuarioRepository usuarioRepository)
        {
            _reportesRepository = reportesRepository;
            _usuarioRepository = usuarioRepository;

        }

        
        [HttpPost("{service:int}")]
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<ReporteHistorial>>>>> Post([FromBody] ReporteHistorialFiltro filtro, [FromRoute] int service)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));             
            var usuario = response.Data;
            var icod = service ==(int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ReporteHistorial(filtro, service, icod);
        }

        [HttpPost("excel/{service:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<List<ReporteHistorial>>>> PostExcel([FromBody] ReporteHistorialFiltro filtro, [FromRoute] int service)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ReporteHistorialExcel(filtro, service, icod);
        }



        [HttpPost("placas/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<Placa>>>> GetPlacas([FromRoute] int service,[FromBody] ReporteHistorialFiltro filtro)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ListPlaca(service,filtro, icod);
        }

        [HttpPost("modelos/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<Modelo>>>> GetModelos([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ListModelo(service, filtro, icod);
        }

        [HttpPost("marcas/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<Marca>>>> GetMarcas([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ListMarca(service, filtro, icod);
        }

        [HttpPost("or/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<OrdenReparacion>>>> GetOR([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));
            var usuario = response.Data;
            var icod = service == (int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ListOR(service, filtro, icod);
        }

        [HttpGet("cliente/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<Cliente>>>> GetClientes([FromRoute] int service)
        {
            
            return await _reportesRepository.ListCliente(service);
        }
    }
}
