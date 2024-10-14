using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class EquipoRepository(WebsitosSredContext context): Repository<Equipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public List<Equipo> GetEquipos()
		{
			return Context.Equipo.ToList();
		}
		public Equipo? GetEquipo(int id)
		{
			return Context.Equipo.Include(x => x.TipoEquipoIdTipoEquipo).Include(x=>x.AulaIdAula).FirstOrDefault(x => x.IdEquipo == id);
		}
	}
}
