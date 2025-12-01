using Entidades.DTOs;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace AseguradoraApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsignarSegurosAseguradosController : Controller
    {
        private readonly AsignarSegurosAseguradoServicio _asignarSegurosAseguradoServicio;

        public AsignarSegurosAseguradosController(AsignarSegurosAseguradoServicio asignarSegurosAseguradoServicio)
        {
            _asignarSegurosAseguradoServicio = asignarSegurosAseguradoServicio;
        }

        //CONTROLADOR PARA ASIGNAR UN SEGURO A UN ASEGURADO
        [HttpPost("AsignarSegurosAsegurados")]
        public async Task<IActionResult> AsignarSeguro([FromBody] AsignacionSegurosAsegurados asignacionSegurosAsegurados)
        {
            try
            {
                await _asignarSegurosAseguradoServicio.AsignarSegurosAsegurados(asignacionSegurosAsegurados.AseguradoId, asignacionSegurosAsegurados.SeguroId);
                return Ok(new{ mensaje = "Seguro asignado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}