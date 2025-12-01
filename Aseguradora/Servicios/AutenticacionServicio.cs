using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class AutenticacionServicio
    {
        private readonly IAutenticacionRepositorio _autenticacionRepositorio;

        public AutenticacionServicio(IAutenticacionRepositorio autenticacionRepositorio)
        {
            _autenticacionRepositorio = autenticacionRepositorio;
        }

        //LOGICA DE NEGOCIO PARA REGISTRO DE USUARIO
        public async Task RegistrarUsuario(Autenticacion autenticacion)
        {
            try
            {
                //validaciones para los campos de registro
                if (autenticacion == null)
                    throw new ArgumentNullException(nameof(autenticacion), "Los datos no pueden ser nulo.");

                if (string.IsNullOrWhiteSpace(autenticacion.nombreCompleto))
                    throw new ArgumentException("El nombre de usuario es requerido.");

                if (string.IsNullOrWhiteSpace(autenticacion.Correo))
                    throw new ArgumentException("El correo es requerido");

                if (string.IsNullOrWhiteSpace(autenticacion.clave))
                    throw new ArgumentException("La contraseña es requerida.");

                await _autenticacionRepositorio.RegistrarUsuario(autenticacion);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA LOGIN DE USUARIO
        public async Task<bool> LoginUsuario(string correo, string clave)
        {
            try
            {
                //validaciones para los campos de login
                if (string.IsNullOrWhiteSpace(correo))
                    throw new ArgumentException("El nombre de usuario es requerido.");

                if (string.IsNullOrWhiteSpace(clave))
                    throw new ArgumentException("La contraseña es requerida.");

                var esValido = await _autenticacionRepositorio.ValidarCredenciales(correo, clave);
                return esValido;
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }
    }
}
