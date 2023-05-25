using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Pagination;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.EntidadesSinLlaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Interfaces
{
    public interface IReportesRepository
    {
        Task<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>> ReporteHistorial(ReporteHistorialFiltro filtro);
    }
}
