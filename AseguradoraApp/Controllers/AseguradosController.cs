using Entidades.Modelos;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace AseguradoraApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AseguradosController : Controller
    {
        private readonly AseguradoServicio _aseguradoServicio;
        
        public AseguradosController(AseguradoServicio aseguradoServicio)
        {
            _aseguradoServicio = aseguradoServicio;
        }

        //CONTROLADOR PARA CARGAR TODOS LOS ASEGURADOS
        [HttpGet("ObtenerAsegurados")]
        public async Task<IActionResult> ObtenerAsegurados()
        {
            try
            {
                var asegurados = await _aseguradoServicio.ObtenerAsegurados();
                return Ok(asegurados);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA REALIZAR LA CONSULTA DE LOS SEGUROS POR LA CEDULA DEL ASEGURADO
        [HttpGet("consultaSeguroPorCedula/{cedula}")]
        public async Task<IActionResult> ConsultaSeguroPorCedula(string cedula)
        {
            try
            {
                var seguros = await _aseguradoServicio.ConsultarSegurosPorAsegurado(cedula); 
                
                if (seguros == null || !seguros.Any()) 
                { 
                    return NotFound(new { mensaje = "No hay seguros registrados." });
                }
                return Ok(seguros); 
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA PODER INSERTAR UN NNUEVO ASEGURADO        
        [HttpPost("InsertarAsegurado")]
        public async Task<IActionResult> InsertarAsegurado([FromBody] Asegurados asegurados, [FromQuery] string usuario)
        {
            try
            {
                await _aseguradoServicio.InsertarAsegurado(asegurados, usuario);
                return Ok(new { mensaje = "Asegurado insertado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA PODER ACTUALIZAR UN ASEGURADO
        [HttpPut("ActualizarAsegurado")]
        public async Task<IActionResult> ActualizarAsegurado([FromBody] Asegurados asegurado, [FromQuery] string usuario)
        {
            try
            {
                await _aseguradoServicio.ActualizarAsegurado(asegurado, usuario);
                return Ok(new { mensaje = "Asegurado actualizado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA ELIMINAR UN ASEGURADO 
        [HttpDelete("EliminarAsegurado/{AseguradoId}")]
        public async Task<IActionResult> EliminarAsegurado(int AseguradoId)
        {
            try
            {
                await _aseguradoServicio.EliminarAsegurado(AseguradoId);
                return Ok(new { mensaje = "Asegurado eliminado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA INSERTAR ASEGURADOS DE MANERA MASIVA
        [HttpPost("InsertarAseguradosMasivo")]
        public async Task<IActionResult> InsertarAseguradosMasivo([FromBody] List<Asegurados> asegurado, [FromQuery] string usuario)
        {
            try
            {
                await _aseguradoServicio.InsertarAseguradosMasivamente(asegurado, usuario);
                return Ok(new { mensaje = "Asegurados insertados correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new {mensaje = ex.Message});
            }
        }
    }
}