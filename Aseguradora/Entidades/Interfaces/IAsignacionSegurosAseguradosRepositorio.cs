namespace Entidades.Interfaces
{
    public interface IAsignacionSegurosAseguradosRepositorio
    {
        Task AsignarSegurosAsegurados(int aseguradoId, int SeguroId);
    }
}
