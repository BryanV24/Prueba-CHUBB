using Entidades.Modelos;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace AseguradoraApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : Controller
    {
        private readonly AutenticacionServicio _autenticacionServicio;

        public AutenticacionController(AutenticacionServicio autenticacionServicio)
        {
            _autenticacionServicio = autenticacionServicio;
        }

        //CONTROLADOR PARA REGISTRAR UN USUARIO
        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] Autenticacion autenticacion)
        {
            try
            {
                await _autenticacionServicio.RegistrarUsuario(autenticacion);
                return Ok(new { mensaje = "Usuario registrado exitosamente." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //CONTROLADOR PARA EL LOGIN DE USUARIO
        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario([FromBody] Autenticacion autenticacion)
        {
            try
            {
                var esValido = await _autenticacionServicio.LoginUsuario(autenticacion.Correo!, autenticacion.clave!);
                if (esValido)
                {
                    return Ok(new { mensaje = "Login exitoso." });
                }
                else
                {
                    return Unauthorized("Credenciales inválidas.");
                }
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}