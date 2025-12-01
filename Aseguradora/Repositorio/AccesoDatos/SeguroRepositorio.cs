using Entidades.DTOs;
using Entidades.Interfaces;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;
using Repositorio.Dbconexion;
using Repositorio.MapeoExepciones;
using System.Data;

namespace Repositorio.AccesoDatos
{
    public class SeguroRepositorio : ISeguroRepositorio
    {
        private readonly ConexionDB _conexion;

        public SeguroRepositorio (ConexionDB conexion)
        {
            _conexion = conexion;
        }

        //CONSULTA DE TODOS LOS SEGUROS
        public async Task<IEnumerable<Seguros>> ConsultarSeguros()
        {
            var ListaSeguros = new List<Seguros>();
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_ObtenerSeguros", conexion) { CommandType = CommandType.StoredProcedure };
            try
            {
                await conexion.OpenAsync();
                using var lector = await comando.ExecuteReaderAsync();
                while (await lector.ReadAsync())
                {
                    var seguro = new Seguros
                    {
                        SeguroId = Convert.ToInt32(lector["SeguroId"]),
                        NombreSeguro = lector["NombreSeguro"].ToString(),
                        CodigoSeguro = lector["CodigoSeguro"].ToString(),
                        SumaAsegurada = Convert.ToDecimal(lector["SumaAsegurada"]),
                        Prima = Convert.ToDecimal(lector["Prima"])
                    };
                    ListaSeguros.Add(seguro);
                }
                return ListaSeguros;
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

        //CONSULTA DE ASEGURADOS POR CODIGO DE SEGURO
        public async Task<IEnumerable<AseguradosPorCodigoSeguro>> ConsultarAseguradosPorSeguro(string codigoSeguro)
        {
            var ListaAsegurado = new List<AseguradosPorCodigoSeguro>();
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_ConsultarAseguradosPorSeguro", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@CodigoSeguro", codigoSeguro);

            try
            {
                await conexion.OpenAsync();
                using var lector = await comando.ExecuteReaderAsync();
                while (await lector.ReadAsync())
                {
                    var asegurado = new AseguradosPorCodigoSeguro
                    {
                        CodigoSeguro = lector["CodigoSeguro"].ToString(),
                        NombreSeguro = lector["NombreSeguro"].ToString(),
                        Cedula = lector["Cedula"].ToString(),
                        NombreCompleto = lector["NombreCompleto"].ToString(),
                        Telefono = lector["Telefono"].ToString(),
                        Edad = Convert.ToInt32(lector["Edad"])
                    };
                    ListaAsegurado.Add(asegurado);
                }
                return ListaAsegurado;
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

        //INSERCCION DE NUEVOS SEGUROS
        public async Task InsertarSeguro(Seguros seguro, string usuario)
        {
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_InsertarSeguro", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@NombreSeguro", seguro.NombreSeguro);
            comando.Parameters.AddWithValue("@CodigoSeguro", seguro.CodigoSeguro);
            comando.Parameters.AddWithValue("@SumaAsegurada", seguro.SumaAsegurada);
            comando.Parameters.AddWithValue("@Prima", seguro.Prima);
            comando.Parameters.AddWithValue("@UsuarioCreacion", usuario);

            try
            {
                await conexion.OpenAsync();
                await comando.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

        //ACTUALIZACION DE SEGUROS
        public async Task ActualizarSeguro(Seguros seguro, string usuario)
        {
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_ActualizarSeguro", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@SeguroId", seguro.SeguroId);
            comando.Parameters.AddWithValue("@NombreSeguro", seguro.NombreSeguro);
            comando.Parameters.AddWithValue("@SumaAsegurada", seguro.SumaAsegurada);
            comando.Parameters.AddWithValue("@Prima", seguro.Prima);
            comando.Parameters.AddWithValue("@UsuarioModificacion", usuario);

            try
            {
                await conexion.OpenAsync();
                await comando.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

        //ELIMINACION DE SEGUROS
        public async Task EliminarSeguro(int seguroId)
        {
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_EliminarSeguro", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@SeguroId", seguroId);

            try
            {
                await conexion.OpenAsync();
                await comando.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

    }
}
