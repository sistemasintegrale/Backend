using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Infraestructure.Context;
using SGE.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Services
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReportesRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<BaseResponse<ReporteHistorialResponseDto>>> ReporteHistorial(ReporteHistorialFiltro filtro)
        {
            PaginationResponse<BaseResponse<ReporteHistorialResponseDto>> response = new PaginationResponse<BaseResponse<ReporteHistorialResponseDto>>();
            try
            {
                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.SmallDateTime).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.SmallDateTime).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@desde", System.Data.SqlDbType.Int).Value = filtro.desde;
                cmd.Parameters.Add("@hasta", System.Data.SqlDbType.Int).Value = filtro.hasta;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {

                }
            }
            catch (Exception ex)
            {
                response.Data!.IsSucces = false;
                response.Data.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.Data.innerExeption = ex.Message;
            }
            return response;
        }
    }
}
