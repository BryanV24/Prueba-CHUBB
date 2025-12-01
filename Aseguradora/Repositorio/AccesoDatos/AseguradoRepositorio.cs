using Entidades.DTOs;
using Entidades.Interfaces;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;
using Repositorio.Dbconexion;
using Repositorio.MapeoExepciones;
using System.Data;

namespace Repositorio.AccesoDatos
{
    public class AseguradoRepositorio : IAseguradoRepositorio
    {
        public readonly ConexionDB _conexionDB;
        public AseguradoRepositorio(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        //CONSULTA PARA OBTENER TODOS LOS ASEGURADOS
        public async Task<IEnumerable<Asegurados>> ObtenerAsegurados()
        {
            var listaAsegurados = new List<Asegurados>();
            using var conexion = _conexionDB.Conexion();
            using var comando = new SqlCommand("SP_ObtenerAsegurados", conexion) { CommandType = CommandType.StoredProcedure };
            try
            {
                await conexion.OpenAsync();
                using var lector = await comando.ExecuteReaderAsync();
                while (await lector.ReadAsync())
                {
                    var asegurado = new Asegurados
                    {
                        AseguradoId = Convert.ToInt32(lector["AseguradoId"]),
                        NombreCompleto = lector["NombreCompleto"].ToString(),
                        Cedula = lector["Cedula"].ToString(),
                        Telefono = lector["Telefono"].ToString(),
                        Edad = Convert.ToInt32(lector["Edad"]),
                    };
                    listaAsegurados.Add(asegurado);
                }
                return listaAsegurados;
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }

        //CONSULTA DE SEGUROS POR CEDULA DE ASEGURADOS
        public async Task<IEnumerable<SegurosPorCedula>> ConsultarSegurosPorCedula(string cedula)
        {
            var listaSeguros = new List<SegurosPorCedula>();
            using var conexion = _conexionDB.Conexion();
            using var comando = new SqlCommand("SP_ConsultarSegurosPorCedula", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@Cedula", cedula);

            try
            {
                await conexion.OpenAsync();
                using var lector = await comando.ExecuteReaderAsync();
                while (await lector.ReadAsync())
                {
                    var seguro = new SegurosPorCedula
                    {
                        Cedula = lector["Cedula"].ToString(),
                        NombreCompleto = lector["NombreCompleto"].ToString(),
                        NombreSeguro = lector["NombreSeguro"].ToString(),
                        CodigoSeguro = lector["CodigoSeguro"].ToString(),
                        SumaAsegurada = Convert.ToDecimal(lector["SumaAsegurada"]),
                        Prima = Convert.ToDecimal(lector["Prima"]),
                    };
                    listaSeguros.Add(seguro);
                }
                return listaSeguros;
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));

            }
        }
        
        //INSERCCION DE UN NUEVO ASEGURADO
        public async Task InsertarAsegurado(Asegurados asegurado, string usuario)
        {
            using var conexion = _conexionDB.Conexion();
            using var comando = new SqlCommand("SP_InsertarAsegurado", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@NombreCompleto", asegurado.NombreCompleto);
            comando.Parameters.AddWithValue("@Cedula", asegurado.Cedula);
            comando.Parameters.AddWithValue("@Telefono", asegurado.Telefono);
            comando.Parameters.AddWithValue("@Edad", asegurado.Edad);
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

        //ACTUALIZACION DE UN ASEGURADO EXISTENTE
        public async Task ActualizarAsegurado(Asegurados asegurado, string usuario)
        {
            using var conexion = _conexionDB.Conexion();
            using var comando = new SqlCommand("SP_ActualizarAsegurado", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@AseguradoId", asegurado.AseguradoId);
            comando.Parameters.AddWithValue("@NombreCompleto", asegurado.NombreCompleto);
            comando.Parameters.AddWithValue("@Telefono", asegurado.Telefono);
            comando.Parameters.AddWithValue("@Edad", asegurado.Edad);
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

        //ELIMINACION DE UN ASEGURADO
        public async Task EliminarAsegurado(int aseguradoId)
        {
            using var conexion = _conexionDB.Conexion();
            using var comando = new SqlCommand("SP_EliminarAsegurado", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@AseguradoId", aseguradoId);

            try
            {
                await conexion.OpenAsync();
                await comando.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                throw new ApplicationException("Error al eliminar el registro.");
            }
        }

    }
}
