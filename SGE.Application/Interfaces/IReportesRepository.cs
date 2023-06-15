using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Pagination;
using SGE.Domain.Dtos;
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
        Task<PaginationResponse<BaseResponse<List<ReporteHistorial>>>> ReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente);
        Task<BaseResponse<List<ReporteHistorial>>>  ReporteHistorialExcel(ReporteHistorialFiltro filtro, int service, int cliente);
        Task<BaseResponse<List<Marca>>>ListMarca(int service, ReporteHistorialFiltro filtro, int cliente);
        Task<BaseResponse<List<Modelo>>> ListModelo(int service, ReporteHistorialFiltro filtro, int cliente);
        Task<BaseResponse<List<Placa>>> ListPlaca(int service, ReporteHistorialFiltro filtro, int cliente);
        Task<BaseResponse<List<OrdenReparacion>>> ListOR(int service, ReporteHistorialFiltro filtro, int cliente);
        Task<BaseResponse<List<Cliente>>> ListCliente(int service);
    }
}
