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
		public async Task<AulaConEquiposDTO> GetEquiposByAulaId(int aulaid)
		{
			var aula = await Context.Aula.Include(a => a.Equipo).Where(x => x.IdAula == aulaid)
				.Select(x => new AulaConEquiposDTO
				{
					Id = x.IdAula,
					Nombre = x.Nombre,
					Equipos = x.Equipo.Select(e=> new EquipoDTO
					{
						Id = e.IdEquipo,
						Numero = e.NumeroIdentificacion,
						TipoId = e.TipoEquipoIdTipoEquipo,
						AulaId = e.AulaIdAula
					}).ToList()
				}).FirstOrDefaultAsync();
			return aula;
		}
	}
}
