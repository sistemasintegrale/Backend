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
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>> ReporteHistorial(ReporteHistorialFiltro filtro)
        {
            PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>> response = new PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>();
            response.Data = new BaseResponse<List<ReporteHistorialResponseDto>>();
            try
            {
                List<ReporteHistorial> listaResultado = new List<ReporteHistorial>();
                var tupla = await Task.WhenAll(ObtenerReporteHistorial(filtro));
                listaResultado = tupla[0].Item1;
                response.Cantidad = tupla[0].Item2;
                response.Data.Data = _mapper.Map<List<ReporteHistorialResponseDto>>(listaResultado);
                response.Data.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.Data!.IsSucces = false;
                response.Data.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.Data.innerExeption = ex.Message;
            }
            return response;
        }

        private async Task<Tuple<List<ReporteHistorial>, int>> ObtenerReporteHistorial(ReporteHistorialFiltro filtro)
        {
            var lista = await CargarRegistrosReporteHistorial(filtro);
            var cantidad = await CargarCantidadRegistrosReporteHistorial(filtro);

            return new Tuple<List<ReporteHistorial>, int>(lista,cantidad);
        }

        private async Task<List<ReporteHistorial>> CargarRegistrosReporteHistorial(ReporteHistorialFiltro filtro)
        {
            return await Task.Run(() =>
            {
                List<ReporteHistorial> listaResultado = new List<ReporteHistorial>();
                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.SmallDateTime).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.SmallDateTime).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@desde", System.Data.SqlDbType.Int).Value = filtro.desde;
                cmd.Parameters.Add("@hasta", System.Data.SqlDbType.Int).Value = filtro.hasta;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {



                    listaResultado.Add(new ReporteHistorial()
                    {
                        Placa = (string)reader["Placa"],
                        NombreCliente = (string)reader["NombreCliente"],
                        Marca = (string)reader["Marca"],
                        Modelo = (string)reader["Modelo"],
                        NumeroOrden = (string)reader["NumeroOrden"],
                        Situacion = (string)reader["Situacion"],
                        NumeroDocumento = (string)reader["NumeroDocumento"],
                        FechaOrden = (string)reader["FechaOrden"],
                        DescripcionTipoServicio = (string)reader["DescripcionTipoServicio"],
                        Kilometraje = (string)reader["Kilometraje"],
                        Cantidad = (decimal)reader["Cantidad"],
                        DescripcionServicio = (string)reader["DescripcionServicio"],
                        PrecioTotalItem = (decimal)reader["PrecioTotalItem"],
                        CodigoMoneda = (int)reader["CodigoMoneda"],
                    });
                }

                conn.Close();
                return listaResultado;
            });
        }

        private async Task<int> CargarCantidadRegistrosReporteHistorial(ReporteHistorialFiltro filtro)
        {
            return await Task.Run(() =>
            {
                int Cantidad = 0;
                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA_CANTIDAD_REGISTROS";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.SmallDateTime).Value =filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.SmallDateTime).Value = filtro.fechaHasta;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Cantidad = (int)reader["Cantidad"];
                }

                conn.Close();
                return Cantidad;
            });
        }
    }
}
