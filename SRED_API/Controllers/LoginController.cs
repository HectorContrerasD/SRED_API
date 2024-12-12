using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRED_API.Helpers;
using SRED_API.Models.DTOs;
using SRED_API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace SRED_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsuarioReporisitory _repository;
        private readonly JWTHelper _jwthelper;
        private readonly IHttpClientFactory httpClient;
        public LoginController(UsuarioReporisitory reporisitory, JWTHelper jWTHelper, IHttpClientFactory httpClient)
        {
            _repository = reporisitory;
            _jwthelper = jWTHelper;
            this.httpClient = httpClient;
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
            var token = _jwthelper.GetToken(usuario.Nombre, rol, EncryptionHelper.StringToSHA512(usuarioDTO.Contrasena));
            
            return Ok(token);
        }
        [HttpPost("UserLog")]
        public async Task<IActionResult> UserLogin(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null){return BadRequest();}
            if (string.IsNullOrWhiteSpace(usuarioDTO.Usuario)) return BadRequest("Ingrese el usuario. ");
            if (string.IsNullOrWhiteSpace(usuarioDTO.Contrasena)) return BadRequest("Ingrese la contraseña. ");
            bool esNumTrabajo = usuarioDTO.Usuario.All(char.IsDigit);
            string url;
            HttpResponseMessage resp;
            if (esNumTrabajo)
            {
                url = $"docentes/datosgenerales?control={usuarioDTO.Usuario}&password={usuarioDTO.Contrasena}";
                using HttpClient Client = httpClient.CreateClient("client");
                resp = await Client.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    return BadRequest(resp.Content.ReadAsStringAsync().Result);

                }
            }
            else
            {
                var path = $"alumno/datosgenerales?control={usuarioDTO.Usuario.ToUpper()}&password={usuarioDTO.Contrasena}";
                using HttpClient Client = httpClient.CreateClient("client");
                resp = await Client.GetAsync(path);
                if (!resp.IsSuccessStatusCode)
                {
                    return BadRequest(resp.Content.ReadAsStringAsync().Result);

                }
            }
            string rol = "Invitado";
            var token = _jwthelper.GetToken(usuarioDTO.Usuario, rol, usuarioDTO.Contrasena);
            return Ok(token);
        }
    }
}
