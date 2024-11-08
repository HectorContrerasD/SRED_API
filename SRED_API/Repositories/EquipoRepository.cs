using Microsoft.EntityFrameworkCore;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class EquipoRepository(WebsitosSredContext context): Repository<Equipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task< List<EquipoDatosDto> >GetEquipos()
		{
			var equipo= await Context.Equipo.Include(x=>x.AulaIdAulaNavigation).Select(x=>new EquipoDatosDto()
			{
				Aula=x.AulaIdAulaNavigation.Nombre,
				Nombre=x.NumeroIdentificacion,
				Id=x.IdEquipo
			}).ToListAsync();
			return equipo;
		}
		public async Task< Equipo?> GetEquipo(int id)
		{
			return await Context.Equipo.Include(x => x.TipoEquipoIdTipoEquipo).Include(x=>x.AulaIdAula).FirstOrDefaultAsync(x => x.IdEquipo == id);
		}
	}
}
