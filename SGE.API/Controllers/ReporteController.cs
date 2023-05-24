using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;

namespace SGE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReportesRepository reportesRepository;

        public ReporteController(IReportesRepository reportesRepository)
        {
            this.reportesRepository = reportesRepository;
        }

        [HttpPost]
        public ActionResult Pos(ReporteHistorialFiltro filtro)
        {
            var dara = reportesRepository.ReporteHistorial(filtro);
            return Ok();
        }
    }
}
