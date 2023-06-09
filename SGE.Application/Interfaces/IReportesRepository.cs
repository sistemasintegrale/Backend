using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Pagination;
using SGE.Domain.Dtos;
using SGE.Domain.Dtos.Marca;
using SGE.Domain.Dtos.Modelo;
using SGE.Domain.Dtos.OrdenReparacion;
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
        Task<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>> ReporteHistorial(ReporteHistorialFiltro filtro, int service);        
        Task<BaseResponse<List<MarcaDto>>>ListMarca(int service, ReporteHistorialFiltro filtro);
        Task<BaseResponse<List<ModeloDto>>> ListModelo(int service, ReporteHistorialFiltro filtro);
        Task<BaseResponse<List<PlacaDto>>> ListPlaca(int service, ReporteHistorialFiltro filtro);
        Task<BaseResponse<List<OrdenReparacionDto>>> ListOR(int service, ReporteHistorialFiltro filtro);
    }
}
