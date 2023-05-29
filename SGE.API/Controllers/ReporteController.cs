using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
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

        [HttpPost]
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>>> Post([FromBody]ReporteHistorialFiltro filtro)
        {
            return await  _reportesRepository.ReporteHistorial(filtro);             
        }

        [HttpPost("motos")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>>> PostMotos([FromBody] ReporteHistorialFiltro filtro)
        {
            return await _reportesRepository.ReporteHistorialMotos(filtro);
        }
    }
}
