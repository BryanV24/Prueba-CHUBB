using Entidades.Modelos;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace AseguradoraApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SegurosController : Controller
    {
        private readonly SeguroServicio _seguroServicio;

        public SegurosController(SeguroServicio seguroServicio)
        {
            _seguroServicio = seguroServicio;
        }

        //CONTROLADOR PARA CONSULTAR TODOS LOS SEGUROS
        [HttpGet("ObtenerSeguros")]
        public async Task<IActionResult> ConsultarSeguros()
        {
            try
            {
                var seguros = await _seguroServicio.ConsultarSeguros();
                return Ok(seguros);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA CONSULTAR LOS ASEGURADOS POR EL CODIGO DE SEGURO
        [HttpGet("ConsultarAseguradoPorCodigoSeguro/{codigoSeguro}")]
        public async Task<IActionResult> ConsultarAseguradoPorCodigoSeguro(string codigoSeguro)
        {
            try
            {
                var asegurados = await _seguroServicio.ConsultarAseguradoPorCodigoSeguro(codigoSeguro);
                if (asegurados == null || !asegurados.Any())
                {
                    return NotFound(new { mensaje = "No hay asegurados registrados para el código de seguro proporcionado." });
                }
                return Ok(asegurados);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });

            }
        }

        //CONTROLADOR PARA INSERTAR UN NUEVO SEGURO
        [HttpPost("InsertarSeguro")]
        public async Task<IActionResult> InsertarSeguro([FromBody] Seguros seguros, [FromQuery] string usuario)
        {
            try
            {
                await _seguroServicio.InsertarSeguro(seguros, usuario);
                return Ok(new { mensaje = "Seguro insertado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA ACTUALIZAR UN SEGURO
        [HttpPut("ActualizarSeguro")]
        public async Task<IActionResult> AxctualizarSeguro([FromBody] Seguros seguro, [FromQuery] string usuario)
        {
            try
            {
                await _seguroServicio.ActualizarSeguro(seguro, usuario);
                return Ok(new { mensaje = "Seguro actualizado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA PODER ELIMNAR UN SEGURO
        [HttpDelete("EliminarSeguro/{seguroId}")]
        public async Task<IActionResult> EliminarSeguro(int seguroId)
        {
            try
            {
                await _seguroServicio.EliminarSeguro(seguroId);
                return Ok(new { mensaje = "Seguro eliminado correctamente." });
            }
            catch(ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        //CONTROLADOR PARA INSERTAR SEGUROS DE MANERA MASIVA
        [HttpPost("InsertarSegurosMasivo")]
        public async Task<IActionResult> InsertarSegurosMasivo([FromBody] List<Seguros> seguros, [FromQuery] string usuario)
        {
            try
            {
                await _seguroServicio.InsertarSegurosMasivamente(seguros, usuario);
                return Ok(new { mensaje = "Seguros insertados correctamente." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
