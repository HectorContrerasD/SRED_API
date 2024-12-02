using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
    public class UsuarioReporisitory(WebsitosSredContext context) : Repository<Usuario>(context)
    {
        private readonly WebsitosSredContext Context = context;
        public async Task<Usuario?> GetUsuario(string usuario, string contrasena)
        {
            return await Context.Usuario.Where(x => x.Nombre == usuario && x.Contraseña == contrasena).FirstOrDefaultAsync();
        }
    }
}
