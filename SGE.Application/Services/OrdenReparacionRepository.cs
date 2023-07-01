using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Infraestructure.Context;
using SGE.Utilities.Enum;
using SGE.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Application.Services
{
    public class OrdenReparacionRepository : IOrdenReparacionRepository
    {
        private readonly NovaMotosDbContext _novaMotosDbContext;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrdenReparacionRepository(ApplicationDbContext context, IMapper mapper, NovaMotosDbContext novaMotosDbContext)
        {
            _context = context;
            _mapper = mapper;
            _novaMotosDbContext = novaMotosDbContext;
        }
        public async Task<PaginationResponse<BaseResponse<List<OrdenReparacionList>>>> OrdenReparacion(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            PaginationResponse<BaseResponse<List<OrdenReparacionList>>> response = new PaginationResponse<BaseResponse<List<OrdenReparacionList>>>();
            response.Data = new BaseResponse<List<OrdenReparacionList>>();
            try
            {

                var tupla = await Task.WhenAll(ObtenerOrdenReparacion(filtro, service, cliente));
                response.Data.Data = tupla[0].Item1;
                response.Cantidad = tupla[0].Item2;
                response.Data.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.Data!.IsSucces = false;
                response.Data.Mensaje = ReplyMessage.MESSAGE_FALIED + filtro;
                response.Data.innerExeption = ex.Message;
            }
            return response;
        }

        private async Task<Tuple<List<OrdenReparacionList>, int>> ObtenerOrdenReparacion(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            var lista = await CargarRegistrosReporteHistorial(filtro, service, cliente);
            var cantidad = await CargarCantidadRegistrosReporteHistorial(filtro, service, cliente);
            return new Tuple<List<OrdenReparacionList>, int>(lista, cantidad);
        }
        private async Task<List<OrdenReparacionList>> CargarRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            return await Task.Run(() =>
            {
                List<OrdenReparacionList> listaResultado = new List<OrdenReparacionList>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_ORDEN_REPARACION_GET_ALL";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@desde", System.Data.SqlDbType.Int).Value = filtro.desde;
                cmd.Parameters.Add("@hasta", System.Data.SqlDbType.Int).Value = filtro.hasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listaResultado.Add(new OrdenReparacionList()
                    {
                        NumeroOrden = (string)reader["NumeroOrden"],
                        NumeroPresupuesto = (string)reader["NumeroPresupuesto"],
                        FechaOrden = ((DateTime)reader["FechaOrden"]).ToString("dd/MM/yyyy"),
                        Cliente = (string)reader["Cliente"],
                        CiaSeguro = (string)reader["CiaSeguro"],
                        Placa = (string)reader["Placa"],
                        Marca = (string)reader["Marca"],
                        Modelo = (string)reader["Modelo"],
                        Motor = (string)reader["Motor"],
                        Anio = (int)reader["Anio"],
                        Situacion = (string)reader["Situacion"],
                        NumeroDocumento = (string)reader["NumeroDocumento"],
                        FechaDocumento = ((DateTime)reader["FechaDocumento"]).ToString("dd/MM/yyyy"),
                        OrdenCompra = (string)reader["OrdenCompra"],
                        Importe = (decimal)reader["Importe"],
                        Moneda = (string)reader["Moneda"]
                    });
                }

                conn.Close();
                return listaResultado;
            });
        }

        private async Task<int> CargarCantidadRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            return await Task.Run(() =>
            {
                int Cantidad = 0;
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_ORDEN_REPARACION_GET_ALL_CANTIDAD_REGISTROS";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Cantidad = (int)reader["Cantidad"];
                }

                conn.Close();
                return Cantidad;
            });
        }


        public Task<BaseResponse<string>> OrdenReparacionExcel(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse<string>> DatosDashboard(int service, int cliente)
        {
            BaseResponse<string> response = new BaseResponse<string>();

            try
            {

                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_DATOS_DASHBOARD";
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var data = (string)reader["DATA"]!;
                    if (string.IsNullOrEmpty(data))
                    {
                        response.IsSucces = false;
                        response.Mensaje = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    }
                    else
                    {
                        response.Data = data; ;
                        response.Mensaje = ReplyMessage.MESSAGE_QUERY;
                    }
                }

                await conn.CloseAsync();


            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message;
            }
            return response;
        }
    }
}
