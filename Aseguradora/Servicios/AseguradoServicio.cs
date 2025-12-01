using Entidades.DTOs;
using Entidades.Interfaces;
using Entidades.Modelos;

namespace Servicios
{
    public class AseguradoServicio
    {
        private readonly IAseguradoRepositorio _aseguradoRepositorio;

        public AseguradoServicio(IAseguradoRepositorio aseguradoRepositorio)
        {
            _aseguradoRepositorio = aseguradoRepositorio;
        }

        //LÓGICA DE NEGOCIO PARA OBTENER TODOS LOS ASEGURADOS
        public async Task<IEnumerable<Asegurados>> ObtenerAsegurados()
        {
            try 
            {
                var asegurados = await _aseguradoRepositorio.ObtenerAsegurados();
                return asegurados;
                
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error al obtener asegurados: {ex.Message}");
            }
        }

        //LÓGICA DE NEGOCIO PARA CONSULTAR SEGUROS POR CEDULA DEL ASEGURADO
        public async Task<IEnumerable<SegurosPorCedula>> ConsultarSegurosPorAsegurado(string cedula)
        {
            try
            {
                //validaciones para el campo de la cedula
                if (string.IsNullOrWhiteSpace(cedula))
                    throw new ArgumentException("Cédula del titular es requerida.");

                if (cedula.Length != 10)
                    throw new ArgumentException("La cédula debe tener exactamente 10 dígitos.");

                if (!cedula.All(char.IsDigit))
                    throw new ArgumentException("La cédula solo debe contener números.");

                var seguros = await _aseguradoRepositorio.ConsultarSegurosPorCedula(cedula);

                return seguros;
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA INSERTAR UN NUEVO ASEGURADO
        public async Task InsertarAsegurado(Asegurados asegurado, string usuario)
        {
            try
            {
                //validaciones para los campos del asegurado
                if (asegurado == null)
                    throw new ArgumentNullException(nameof(asegurado), "El asegurado no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(asegurado.NombreCompleto))
                    throw new ArgumentException("El nombre completo es obligatorio.");

                if (string.IsNullOrWhiteSpace(asegurado.Cedula) || asegurado.Cedula.Length != 10 || !asegurado.Cedula.All(char.IsDigit))
                    throw new ArgumentException("La cédula debe tener exactamente 10 dígitos.");

                if (string.IsNullOrWhiteSpace(asegurado.Telefono) || asegurado.Telefono.Length != 10 || !asegurado.Telefono.StartsWith("09") || !asegurado.Telefono.All(char.IsDigit))
                    throw new ArgumentException("El teléfono debe tener formato 09xxxxxxxx.");

                if (asegurado.Edad < 0 || asegurado.Edad > 120)
                    throw new ArgumentException("La edad debe ser un valor válido entre 0 y 120.");

                await _aseguradoRepositorio.InsertarAsegurado(asegurado, usuario);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA ACTUALIZAR UN ASEGURADO
        public async Task ActualizarAsegurado(Asegurados asegurado, string usuario)
        {
            try
            {
                //validaciones para los campos del asegurado
                if (asegurado == null)
                    throw new ArgumentNullException(nameof(asegurado), "El asegurado no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(asegurado.NombreCompleto))
                    throw new ArgumentException("El nombre completo es obligatorio.");

                if (string.IsNullOrWhiteSpace(asegurado.Cedula) || asegurado.Cedula.Length != 10 || !asegurado.Cedula.All(char.IsDigit))
                    throw new ArgumentException("La cédula debe tener exactamente 10 dígitos numéricos.");

                if (string.IsNullOrWhiteSpace(asegurado.Telefono) || asegurado.Telefono.Length != 10 || !asegurado.Telefono.StartsWith("09") || !asegurado.Telefono.All(char.IsDigit))
                    throw new ArgumentException("El teléfono debe tener formato 09xxxxxxxx.");

                if (asegurado.Edad < 0 || asegurado.Edad > 120)
                    throw new ArgumentException("La edad debe ser un valor válido entre 0 y 120.");

                await _aseguradoRepositorio.ActualizarAsegurado(asegurado, usuario);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA ELIMINAR UN ASEGURADO
        public async Task EliminarAsegurado(int aseguradoId)
        {
            try
            {
                if (aseguradoId <= 0)
                    throw new ArgumentException("El ID del asegurado debe ser un valor positivo.");
                await _aseguradoRepositorio.EliminarAsegurado(aseguradoId);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA INSERTAR ASEGURADOS DE MANERA MASIVA
        public async Task InsertarAseguradosMasivamente(IEnumerable<Asegurados> asegurados, string usuario)
        {
            try
            {
                if (asegurados == null || !asegurados.Any())
                    throw new ArgumentException("La lista de asegurados no puede estar vacía.");
                foreach (var asegurado in asegurados)
                {
                    //validaciones para los campos del asegurado
                    if (string.IsNullOrWhiteSpace(asegurado.NombreCompleto))
                        throw new ArgumentException("El nombre completo es obligatorio.");
                    if (string.IsNullOrWhiteSpace(asegurado.Cedula) || asegurado.Cedula.Length != 10 || !asegurado.Cedula.All(char.IsDigit))
                        throw new ArgumentException("La cédula debe tener exactamente 10 dígitos.");
                    if (string.IsNullOrWhiteSpace(asegurado.Telefono) || asegurado.Telefono.Length != 10 || !asegurado.Telefono.StartsWith("09") || !asegurado.Telefono.All(char.IsDigit))
                        throw new ArgumentException("El teléfono debe tener formato 09xxxxxxxx.");
                    if (asegurado.Edad < 0 || asegurado.Edad > 120)
                        throw new ArgumentException("La edad debe ser un valor válido entre 0 y 120.");

                    await _aseguradoRepositorio.InsertarAsegurado(asegurado, usuario);
                }
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }
    }
}
