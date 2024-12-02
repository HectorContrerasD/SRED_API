using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRED_API.Helpers;
using SRED_API.Models.DTOs;
using SRED_API.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace SRED_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsuarioReporisitory _repository;
        private readonly JWTHelper _jwthelper;
        public LoginController(UsuarioReporisitory reporisitory, JWTHelper jWTHelper)
        {
            _repository = reporisitory;
            _jwthelper = jWTHelper;
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(usuarioDTO.Usuario)) return BadRequest("Ingrese el usuario. ");
            if (string.IsNullOrWhiteSpace(usuarioDTO.Contrasena)) return BadRequest("Ingrese la contraseña. ");
            var usuario = await _repository.GetUsuario(usuarioDTO.Usuario, EncryptionHelper.StringToSHA512( usuarioDTO.Contrasena));
            if (usuario == null) return Unauthorized("Usuario o contraseña incorrectos");
            string rol = ""; 
            if (usuario.Rol == 0)
            {
                rol = "Admin";
            }
            else if (usuario.Rol == 1)
            {
                rol = "Encargado";
            }
            var token = _jwthelper.GetToken(usuario.Nombre, rol, usuario.IdUsuario);
            
            return Ok(token);
        }
    }
}
