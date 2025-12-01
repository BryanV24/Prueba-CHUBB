using Entidades.DTOs;
using Entidades.Modelos;

namespace Entidades.Interfaces
{
    public interface ISeguroRepositorio
    {
        Task<IEnumerable<Seguros>> ConsultarSeguros();
        Task<IEnumerable<AseguradosPorCodigoSeguro>> ConsultarAseguradosPorSeguro(string codigoSeguro);
        Task InsertarSeguro(Seguros seguro, string usuario);
        Task ActualizarSeguro(Seguros seguro, string usuario);
        Task EliminarSeguro(int seguroId);
    }
}
