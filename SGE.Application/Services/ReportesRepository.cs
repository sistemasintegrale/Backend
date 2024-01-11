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
using System.Text.Json;

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

        public async Task<PaginationResponse<BaseResponse<List<ReporteHistorial>>>> ReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            PaginationResponse<BaseResponse<List<ReporteHistorial>>> response = new PaginationResponse<BaseResponse<List<ReporteHistorial>>>();
            response.Data = new BaseResponse<List<ReporteHistorial>>();
            try
            {
                List<ReporteHistorial> listaResultado = new List<ReporteHistorial>();
                var tupla = await Task.WhenAll(ObtenerReporteHistorial(filtro, service, cliente));
                listaResultado = tupla[0].Item1;
                response.Cantidad = tupla[0].Item2;
                response.Data.Data = listaResultado;
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

        private async Task<Tuple<List<ReporteHistorial>, int>> ObtenerReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            var lista = await CargarRegistrosReporteHistorial(filtro, service, cliente);
            var cantidad = await CargarCantidadRegistrosReporteHistorial(filtro, service, cliente);
            return new Tuple<List<ReporteHistorial>, int>(lista, cantidad);
        }

        private async Task<List<ReporteHistorial>> CargarRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
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
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
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

        private async Task<int> CargarCantidadRegistrosReporteHistorial(ReporteHistorialFiltro filtro, int service, int cliente)
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


        public async Task<BaseResponse<List<Marca>>> ListMarca(int service, ReporteHistorialFiltro filtro, int cliente)
        {
            BaseResponse<List<Marca>> response = new BaseResponse<List<Marca>>();
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
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
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
                return lista;
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<Modelo>>> ListModelo(int service, ReporteHistorialFiltro filtro, int cliente)
        {
            BaseResponse<List<Modelo>> response = new BaseResponse<List<Modelo>>();
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
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
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
                return lista;
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<Placa>>> ListPlaca(int service, ReporteHistorialFiltro filtro, int cliente)
        {
            BaseResponse<List<Placa>> response = new BaseResponse<List<Placa>>();
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
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
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
                return lista;
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<OrdenReparacion>>> ListOR(int service, ReporteHistorialFiltro filtro, int cliente)
        {
            BaseResponse<List<OrdenReparacion>> response = new BaseResponse<List<OrdenReparacion>>();
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
                cmd.Parameters.Add("@cliente", System.Data.SqlDbType.Int).Value = cliente;
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
                return lista;
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<List<Cliente>>> ListCliente(int service)
        {
            BaseResponse<List<Cliente>> response = new BaseResponse<List<Cliente>>();
            response.Data = await Task.Run(() =>
            {
                List<Cliente> lista = new List<Cliente>();
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_CLIENTE_GET_ALL";
                cmd.CommandTimeout = 99999999;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Cliente()
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString()!
                    });
                }
                conn.Close();
                return lista;
            });
            response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            return response;
        }

        public async Task<BaseResponse<string>> ReporteHistorialExcel(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            BaseResponse<string> response = new BaseResponse<string>();

            try
            {
                
                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_HISTORIA_POR_PLACA_EXCEL";
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

        public async Task<BaseResponse<string>> ListaORExcel(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            BaseResponse<string> response = new BaseResponse<string>();

            try
            {

                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_ORDEN_REPARACION_EXCEL";
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


        public async Task<BaseResponse<string>> ReporteOR(ReporteHistorialFiltro filtro, int service, int cliente)
        {
            BaseResponse<string> response = new BaseResponse<string>();

            try
            {

                SqlConnection conn = service == (int)Enums.ServiceNovaMotos ? (SqlConnection)_novaMotosDbContext.Database.GetDbConnection() : (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "USP_PRY_ORDEN_REPARACION_LISTA";
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
