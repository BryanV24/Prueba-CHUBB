using Entidades.DTOs;
using Entidades.Interfaces;
using Entidades.Modelos;

namespace Servicios
{
    public class SeguroServicio
    {
        public readonly ISeguroRepositorio _seguroRepositorio;

        public SeguroServicio(ISeguroRepositorio seguroRepositorio)
        {
            _seguroRepositorio = seguroRepositorio;
        }

        //LOGICA DE NEGOCIO PARA CONSULTAR TODOS LOS SEGUROS
        public async Task<IEnumerable<Seguros>> ConsultarSeguros()
        {
            try
            {
                var seguros = await _seguroRepositorio.ConsultarSeguros();
                return seguros;

            }
            catch(ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }
        //LOGICA DE NEGOCIO PARA CONSULTAR LOS ASEGURADOS POR EL CODIGO DE SEGURO
        public async Task<IEnumerable<AseguradosPorCodigoSeguro>> ConsultarAseguradoPorCodigoSeguro(string codigoSeguro)
        {
            try
            {
                //validaciones para el campo del codigo de seguro
                if (string.IsNullOrWhiteSpace(codigoSeguro))
                    throw new ArgumentException("El codigo de seguro del titular es requerida.");

                if (codigoSeguro.Length != 10)
                    throw new ArgumentException("El codigo de Seguro debe tener exactamente 10 dígitos.");

                var asegurados = await _seguroRepositorio.ConsultarAseguradosPorSeguro(codigoSeguro);

                return asegurados;
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA INSERTAR UN NUEVO SEGURO
        public async Task InsertarSeguro(Seguros seguro, string usuario)
        {
            try
            {
                //validaciones para los campos de los seguros
                if (seguro == null)
                    throw new ArgumentNullException(nameof(seguro), "El asegurado no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(seguro.NombreSeguro))
                    throw new ArgumentException("El nombre del seguro es obligatorio.");

                if (string.IsNullOrWhiteSpace(seguro.CodigoSeguro) || seguro.CodigoSeguro.Length != 10)
                    throw new ArgumentException("El codigo del seguro debe tener exactamente 10 dígitos.");

                if ((seguro.SumaAsegurada <= 0))
                    throw new ArgumentException("La suma asegurada debe ser un valor positivo.");

                if (seguro.Prima <= 0)
                    throw new ArgumentException("La prima debe ser un valor positivo.");

                await _seguroRepositorio.InsertarSeguro(seguro, usuario);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA ACTUALIZAR UN SEGURO
        public async Task ActualizarSeguro(Seguros seguro, string usuario)
        {
            try
            {
                //validaciones para los campos de los seguros
                if (seguro == null)
                    throw new ArgumentNullException(nameof(seguro), "El asegurado no puede ser nulo.");

                if (string.IsNullOrWhiteSpace(seguro.NombreSeguro))
                    throw new ArgumentException("El nombre del seguro es obligatorio.");

                if (string.IsNullOrWhiteSpace(seguro.CodigoSeguro) || seguro.CodigoSeguro.Length != 10)
                    throw new ArgumentException("El codigo del seguro debe tener exactamente 10 dígitos.");

                if ((seguro.SumaAsegurada <= 0))
                    throw new ArgumentException("La suma asegurada debe ser un valor positivo.");

                if (seguro.Prima <= 0)
                    throw new ArgumentException("La prima debe ser un valor positivo.");

                await _seguroRepositorio.ActualizarSeguro(seguro, usuario);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }

        //LOGICA DE NEGOCIO PARA ELIMINAR UN SEGURO
        public async Task EliminarSeguro(int seguroId)
        {
            try
            {
                if (seguroId <= 0)
                    throw new ArgumentException("El ID del seguro debe ser un valor positivo.");
                await _seguroRepositorio.EliminarSeguro(seguroId);
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");

            }
        }

        //LOGICA DE NEGOCIO PARA INSERTAR SEGUROS DE MANERA MASIVA
        public async Task InsertarSegurosMasivamente(IEnumerable<Seguros> seguros, string usuario)
        {
            try
            {
                if (seguros == null || !seguros.Any())
                    throw new ArgumentException("La lista de los seguros no puede estar vacía.");
                foreach (var seguro in seguros)
                {
                    //validaciones para los campos de los seguros
                    if (string.IsNullOrWhiteSpace(seguro.NombreSeguro))
                        throw new ArgumentException("El nombre del seguro es obligatorio.");

                    if (string.IsNullOrWhiteSpace(seguro.CodigoSeguro) || seguro.CodigoSeguro.Length != 10)
                        throw new ArgumentException("El codigo del seguro debe tener exactamente 10 dígitos.");

                    if ((seguro.SumaAsegurada <= 0))
                        throw new ArgumentException("La suma asegurada debe ser un valor positivo.");

                    if (seguro.Prima <= 0)
                        throw new ArgumentException("La prima debe ser un valor positivo.");

                    await _seguroRepositorio.InsertarSeguro(seguro, usuario);
                }
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }
    }
}