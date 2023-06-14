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
using SGE.Domain.Dtos.Marca;
using SGE.Domain.Dtos.Modelo;
using SGE.Domain.Dtos.OrdenReparacion;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.Dtos.Usuario;
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
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>>> Post([FromBody] ReporteHistorialFiltro filtro, [FromRoute] int service)
        {
            var identntity = HttpContext.User.Identity as ClaimsIdentity;
            var userclaims = identntity.Claims;
            var id = userclaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await _usuarioRepository.GetById(Convert.ToInt32(id));             
            var usuario = response.Data;
            var icod = service ==(int)Enums.ServiceNovaMotos ? usuario.CodigoClienteNM : usuario.CodigoClienteNG;
            return await _reportesRepository.ReporteHistorial(filtro, service, icod);
        }

 

        [HttpPost("placas/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<PlacaDto>>>> GetPlacas([FromRoute] int service,[FromBody] ReporteHistorialFiltro filtro)
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
        public async Task<ActionResult<BaseResponse<List<ModeloDto>>>> GetModelos([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
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
        public async Task<ActionResult<BaseResponse<List<MarcaDto>>>> GetMarcas([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
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
        public async Task<ActionResult<BaseResponse<List<OrdenReparacionDto>>>> GetOR([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
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
        public async Task<ActionResult<BaseResponse<List<ClienteDto>>>> GetClientes([FromRoute] int service)
        {
            
            return await _reportesRepository.ListCliente(service);
        }
    }
}
