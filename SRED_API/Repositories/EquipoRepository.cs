using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class EquipoRepository(WebsitosSredContext context): Repository<Equipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task< List<Equipo> >GetEquipos()
		{
			return await Context.Equipo.ToListAsync();
		}
		public async Task< Equipo?> GetEquipo(int id)
		{
			return await Context.Equipo.Include(x => x.TipoEquipoIdTipoEquipo).Include(x=>x.AulaIdAula).FirstOrDefaultAsync(x => x.IdEquipo == id);
		}
	}
}
