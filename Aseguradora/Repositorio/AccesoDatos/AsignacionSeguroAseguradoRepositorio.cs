using Entidades.Interfaces;
using Microsoft.Data.SqlClient;
using Repositorio.Dbconexion;
using Repositorio.MapeoExepciones;
using System.Data;

namespace Repositorio.AccesoDatos
{
    public class AsignacionSeguroAseguradoRepositorio : IAsignacionSegurosAseguradosRepositorio
    {
        private readonly ConexionDB _conexion;

        public AsignacionSeguroAseguradoRepositorio(ConexionDB conexion)
        {
            _conexion = conexion;
        }
        public async Task AsignarSegurosAsegurados(int aseguradoId, int seguroId)
        {
            using var conexion = _conexion.Conexion(); 
            using var comando = new SqlCommand("SP_AsignarSeguroAsegurado", conexion) { CommandType = CommandType.StoredProcedure };
            
            comando.Parameters.AddWithValue("@AseguradoId", aseguradoId);
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
