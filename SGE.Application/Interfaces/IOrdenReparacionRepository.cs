using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Pagination;
using SGE.Domain.EntidadesSinLlaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Interfaces
{
    public interface IOrdenReparacionRepository
    {
        Task<PaginationResponse<BaseResponse<List<OrdenReparacionList>>>> OrdenReparacion(ReporteHistorialFiltro filtro, int service, int cliente);
        Task<BaseResponse<List<OrdenReparacionList>>> OrdenReparacionExcel(ReporteHistorialFiltro filtro, int service, int cliente);
    }
}
