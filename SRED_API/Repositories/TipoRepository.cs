using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class TipoRepository(WebsitosSredContext context): Repository<Tipoequipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public List<Tipoequipo> GetTipos()
		{
			return Context.Tipoequipo.ToList();
		}
		public Tipoequipo? GetTipo(int id)
		{
			return Context.Tipoequipo.Include(x => x.Equipo).FirstOrDefault(x => x.IdTipoEquipo == id);
		}
	}
}
