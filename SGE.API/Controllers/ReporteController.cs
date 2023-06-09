using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.Dtos;
using SGE.Domain.Dtos.Marca;
using SGE.Domain.Dtos.Modelo;
using SGE.Domain.Dtos.OrdenReparacion;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.Dtos.Usuario;

namespace SGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReporteController : ControllerBase
    {
        private readonly IReportesRepository _reportesRepository;

        public ReporteController(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        [HttpPost("{service:int}")]
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>>> Post([FromBody] ReporteHistorialFiltro filtro, [FromRoute] int service)
        {
            return await _reportesRepository.ReporteHistorial(filtro, service);
        }

 

        [HttpPost("placas/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<PlacaDto>>>> GetPlacas([FromRoute] int service,[FromBody] ReporteHistorialFiltro filtro)
        {
            return await _reportesRepository.ListPlaca(service,filtro);
        }

        [HttpPost("modelos/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<ModeloDto>>>> GetModelos([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            return await _reportesRepository.ListModelo(service, filtro);
        }

        [HttpPost("marcas/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<MarcaDto>>>> GetMarcas([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            return await _reportesRepository.ListMarca(service, filtro);
        }

        [HttpPost("or/{service:int}")]
        public async Task<ActionResult<BaseResponse<List<OrdenReparacionDto>>>> GetOR([FromRoute] int service, [FromBody] ReporteHistorialFiltro filtro)
        {
            return await _reportesRepository.ListOR(service, filtro);
        }
    }
}
