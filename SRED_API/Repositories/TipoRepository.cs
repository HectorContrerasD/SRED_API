using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class TipoRepository(WebsitosSredContext context): Repository<Tipoequipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task<List<Tipoequipo>> GetTipos()
		{
			return await Context.Tipoequipo.Include(x=>x.Equipo).ToListAsync();
		}
		public async Task< Tipoequipo?> GetTipo(int id)
		{
			return  await Context.Tipoequipo.Include(x => x.Equipo).FirstOrDefaultAsync(x => x.IdTipoEquipo == id);
		}
	}
}
