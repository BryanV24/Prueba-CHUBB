using Entidades.Modelos;

namespace Entidades.Interfaces
{
    public interface IAutenticacionRepositorio
    {
        Task RegistrarUsuario(Autenticacion autenticacion);
        Task<bool> ValidarCredenciales(string usuario, string contrasena);
    }
}
