using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.Dtos;
using SGE.Domain.Dtos.Marca;
using SGE.Domain.Dtos.Modelo;
using SGE.Domain.Dtos.OrdenReparacion;
using SGE.Domain.Dtos.ReporteHistorial;
using SGE.Domain.EntidadesSinLlaves;
using SGE.Infraestructure.Context;
using SGE.Utilities.Enum;
using SGE.Utilities.Static;

namespace SGE.Application.Services
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly NovaMotosDbContext _novaMotosDbContext;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReportesRepository(ApplicationDbContext context, IMapper mapper, NovaMotosDbContext novaMotosDbContext)
        {
            _context = context;
            _mapper = mapper;
            _novaMotosDbContext = novaMotosDbContext;
        }

        public async Task<PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>> ReporteHistorial(ReporteHistorialFiltro filtro, int service)
        {
            PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>> response = new PaginationResponse<BaseResponse<List<ReporteHistorialResponseDto>>>();
            response.Data = new BaseResponse<List<ReporteHistorialResponseDto>>();
            try
            {
                List<ReporteHistorial> listaResultado = new List<ReporteHistorial>();
                var tupla = await Task.WhenAll(ObtenerReporteHistorial(filtro, service));
                listaResultado = tupla[0].Item1;
                response.Cantidad = tupla[0].Item2;
                response.Data.Data = _mapper.Map<List<ReporteHistorialResponseDto>>(listaResultado);
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

        private async Task<Tuple<List<ReporteHistorial>, int>> ObtenerReporteHistorial(ReporteHistorialFiltro filtro, int service)
        {
            var lista = await CargarRegistrosReporteHistorial(filtro,service);
            var cantidad = await CargarCantidadRegistrosReporteHistorial(filtro,service);

            return new Tuple<List<ReporteHistorial>, int>(lista, cantidad);
        }

        private async Task<List<ReporteHistorial>> CargarRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service)
        {
            return await Task.Run(() =>
            {
                List<ReporteHistorial> listaResultado = new List<ReporteHistorial>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@desde", System.Data.SqlDbType.Int).Value = filtro.desde;
                cmd.Parameters.Add("@hasta", System.Data.SqlDbType.Int).Value = filtro.hasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.CommandTimeout = 99999999;
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

        private async Task<int> CargarCantidadRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service)
        {
            return await Task.Run(() =>
            {
                int Cantidad = 0;
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA_CANTIDAD_REGISTROS";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
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
 

        public async Task<BaseResponse<List<MarcaDto>>> ListMarca(int service, ReporteHistorialFiltro filtro)
        {
            BaseResponse<List<MarcaDto>> response = new BaseResponse<List<MarcaDto>>();
            response.Data = await Task.Run(() =>
            {
                List<Marca> lista = new List<Marca>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_MARCA_LIST";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Marca()
                    {
                        Id = (int)reader["Id"],
                        Descripcion = reader["Descripcion"].ToString()!
                    });
                }
                conn.Close();
                return _mapper.Map<List<MarcaDto>>(lista);
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<ModeloDto>>> ListModelo(int service, ReporteHistorialFiltro filtro)
        {
            BaseResponse<List<ModeloDto>> response = new BaseResponse<List<ModeloDto>>();
            response.Data = await Task.Run(() =>
            {
                List<Modelo> lista = new List<Modelo>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_MODELO_LIST";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Modelo()
                    {
                        Id = (int)reader["Id"],
                        Descripcion = reader["Descripcion"].ToString()!
                    });
                }
                conn.Close();
                return _mapper.Map<List<ModeloDto>>(lista);
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<PlacaDto>>> ListPlaca(int service, ReporteHistorialFiltro filtro)
        {
            BaseResponse<List<PlacaDto>> response = new BaseResponse<List<PlacaDto>>();
            response.Data = await Task.Run(() =>
            {
                List<Placa> lista = new List<Placa>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_PLACA_LIST";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Placa()
                    {
                        Id = (int)reader["Id"],
                        Descripcion = reader["Descripcion"].ToString()!
                    });
                }
                conn.Close();
                return _mapper.Map<List<PlacaDto>>(lista);
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<OrdenReparacionDto>>> ListOR(int service, ReporteHistorialFiltro filtro)
        {
            BaseResponse<List<OrdenReparacionDto>> response = new BaseResponse<List<OrdenReparacionDto>>();
            response.Data = await Task.Run(() =>
            {
                List<OrdenReparacion> lista = new List<OrdenReparacion>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_OR_LIST";
                cmd.Parameters.Add("@fechaDesde", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaDesde;
                cmd.Parameters.Add("@fechaHasta", System.Data.SqlDbType.VarChar, 10).Value = filtro.fechaHasta;
                cmd.Parameters.Add("@marca", System.Data.SqlDbType.Int).Value = filtro.marca;
                cmd.Parameters.Add("@modelo", System.Data.SqlDbType.Int).Value = filtro.modelo;
                cmd.Parameters.Add("@placa", System.Data.SqlDbType.Int).Value = filtro.placa;
                cmd.Parameters.Add("@orden", System.Data.SqlDbType.Int).Value = filtro.orden;
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new OrdenReparacion()
                    {
                        Id = (int)reader["Id"],
                        Numero = reader["Numero"].ToString()!
                    });
                }
                conn.Close();
                return _mapper.Map<List<OrdenReparacionDto>>(lista);
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }
    }
}
