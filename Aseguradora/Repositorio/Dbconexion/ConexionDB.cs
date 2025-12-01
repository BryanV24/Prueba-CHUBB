using Microsoft.Data.SqlClient;

namespace Repositorio.Dbconexion
{
    public class ConexionDB
    {
        private readonly string _connectionString;
        public ConexionDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SqlConnection Conexion()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
