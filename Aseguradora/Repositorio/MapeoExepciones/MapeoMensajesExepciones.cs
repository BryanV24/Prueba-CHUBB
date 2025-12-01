using Microsoft.Data.SqlClient;

namespace Repositorio.MapeoExepciones
{
    public static class MapeoMensajesExepciones
    {
        //se crea esta clase para poder mapear los mensajes de error de sql a mensajes que los clientes puedan comprender el error
        public static string Map(SqlException ex)
        {
            var msg = ex.Message;

            if (msg.Contains("CK_ValidacionSumaAsegurada") || msg.Contains("CK_SumaPositiva"))
                return "La suma asegurada debe ser un valor positivo y mayor que 0.";
            if (msg.Contains("CK_ValidacionPrima") || msg.Contains("CK_PrimaPositiva"))
                return "La prima debe ser un valor positivo y mayor que 0.";
            if (msg.Contains("CK_ValidacionCedula"))
                return "La cédula debe tener 10 dígitos.";
            if (msg.Contains("CK_ValidacionTelefono"))
                return "El teléfono debe tener formato 09xxxxxxxx.";
            if (msg.Contains("CK_ValidacionEdad"))
                return "Edad inválida.";
            if (msg.Contains("UQ_CodigoSeguro") || msg.Contains("CodigoSeguro") || msg.Contains("UQ__Seguros"))
                return "Ya existe un seguro con ese codigo.";
            if (msg.Contains("UQ_Cedula") || msg.Contains("Cedula"))
                return "Ya existe un asegurado con esa cedula.";

            // fallback
            return "Error en la base de datos: " + ex.Message;
        }
    }
}