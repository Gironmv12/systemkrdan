using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudUsuariosClientesApi.Models;
using CrudUsuariosClientesApi.Data;
using System.Threading.Tasks;

namespace CrudUsuariosClientesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }



        //POST api/usuarios/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.Id }, usuario);
        }

        //GET api/usuarios/
        [HttpGet("/")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        //GET api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        //PUT api/usuarios/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Asignamos las propiedades que sí se pueden modificar
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Password = usuario.Password;
            usuarioExistente.Rol = usuario.Rol;
            usuarioExistente.IsDeleted = usuario.IsDeleted;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                Mensaje = "Usuario actualizado correctamente",
                Datos = usuario
            });
        }

        //DELETE api/usuarios/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id, [FromQuery] bool eliminarFisico = false)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            if (eliminarFisico)
            {
                // Eliminar físicamente de la base de datos
                _context.Usuarios.Remove(usuario);
            }
            else
            {
                // Eliminación lógica
                usuario.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new
            {
                Mensaje = eliminarFisico ? "Usuario eliminado físicamente" : "Usuario eliminado lógicamente",
                Datos = usuario
            });
        }

        //OBTENER USUARIOS CON ELIMINADO LOGICO
        [HttpGet("usuarios-eliminados")]
        public async Task<IActionResult> GetUsuariosEliminados()
        {
            var usuariosEliminados = await _context.Usuarios
                .Where(u => u.IsDeleted)
                .ToListAsync();
            return Ok(usuariosEliminados);

        }
    }
}
