using Entidades.Interfaces;
using Entidades.Modelos;
using Microsoft.Data.SqlClient;
using Repositorio.Dbconexion;
using Repositorio.MapeoExepciones;
using System.Data;

namespace Repositorio.AccesoDatos
{
    public class AutenticacionRepositorio : IAutenticacionRepositorio
    {
        private readonly ConexionDB _conexion;

        public AutenticacionRepositorio(ConexionDB conexion)
        {
            _conexion = conexion;
        }
        public async Task RegistrarUsuario(Autenticacion autenticacion)
        {
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("SP_RegistroUsuario", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@NombreCompleto", autenticacion.nombreCompleto);
            comando.Parameters.AddWithValue("@Correo", autenticacion.Correo);
            comando.Parameters.AddWithValue("@Clave", autenticacion.clave);

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

        public async Task<bool> ValidarCredenciales(string correo, string clave)
        {
            using var conexion = _conexion.Conexion();
            using var comando = new SqlCommand("sp_LoginUsuario", conexion) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.AddWithValue("@Correo", correo);
            comando.Parameters.AddWithValue("@Clave", clave);

            try
            {
                await conexion.OpenAsync();
                var resultado = await comando.ExecuteScalarAsync();
                return Convert.ToInt32(resultado) > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception(MapeoMensajesExepciones.Map(ex));
            }
        }
    }
}
