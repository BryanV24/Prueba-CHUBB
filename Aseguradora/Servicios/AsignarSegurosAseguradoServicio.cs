using Entidades.Interfaces;
using Entidades.Modelos;

namespace Servicios
{
    public class AsignarSegurosAseguradoServicio
    {
        private readonly IAsignacionSegurosAseguradosRepositorio _asignacionSegurosAseguradosRepositorio;

        public AsignarSegurosAseguradoServicio(IAsignacionSegurosAseguradosRepositorio asignacionSegurosAseguradosRepositorio)
        {
            _asignacionSegurosAseguradosRepositorio = asignacionSegurosAseguradosRepositorio;
        }

        //LOGICA DE NEGOCIO PARA ASIGNAR SEGUROS A ASEGURADO
        public async Task AsignarSegurosAsegurados(int idAsegurado, List<int> segurosId)
        {
            try
            {
                //validaciones para los campos de la asignacion
                if (idAsegurado <= 0)
                    throw new ArgumentException("El ID del asegurado debe ser un valor positivo.");

                if (segurosId == null || !segurosId.Any())
                    throw new ArgumentException("Datos inválidos");

                foreach (var seguroId in segurosId)
                {
                    if (seguroId != 0)
                    {
                        await _asignacionSegurosAseguradosRepositorio.AsignarSegurosAsegurados(idAsegurado, seguroId);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Error de validación: {ex.Message}");
            }
        }
    }
}
