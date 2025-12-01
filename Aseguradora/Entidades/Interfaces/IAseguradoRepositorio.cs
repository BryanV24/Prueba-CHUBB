using Entidades.DTOs;
using Entidades.Modelos;

namespace Entidades.Interfaces
{
    public interface IAseguradoRepositorio
    {
        Task<IEnumerable<Asegurados>> ObtenerAsegurados();
        Task<IEnumerable<SegurosPorCedula>> ConsultarSegurosPorCedula(string cedula);
        Task InsertarAsegurado (Asegurados asegurado, string usuario);
        Task ActualizarAsegurado(Asegurados asegurado, string usuario);
        Task EliminarAsegurado(int aseguradoId);
    }
}
