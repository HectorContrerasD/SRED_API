using Microsoft.EntityFrameworkCore;
using SRED_API.Helpers;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class EquipoRepository(WebsitosSredContext context): Repository<Equipo>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task< List<EquipoDatosDto> >GetEquipos()
		{
			var equipo= await Context.Equipo.Include(x=>x.AulaIdAulaNavigation).Where(x => x.Estado == 1).Include(x=>x.TipoEquipoIdTipoEquipoNavigation).Select(x=>new EquipoDatosDto()
			{
				Aula=x.AulaIdAulaNavigation.Nombre,
				Nombre=x.NumeroIdentificacion,
				Id=x.IdEquipo,
				Tipo=x.TipoEquipoIdTipoEquipoNavigation.Nombre??""
			}).ToListAsync();
			return equipo;
		}
		public async Task< EquipoDatosDto?> GetEquipo(int id)
		{
			return await Context.Equipo.Include(x=>x.AulaIdAulaNavigation).
				Select(x=> new EquipoDatosDto{
				Aula = x.AulaIdAulaNavigation.Nombre,
				Id=x.IdEquipo,
				Nombre = x.NumeroIdentificacion
			}).FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<AulaConEquiposDTO> GetEquiposByAulaId(int aulaid)
		{
			var aula = await Context.Aula.Include(a => a.Equipo).Where(x => x.IdAula == aulaid)
				.Select(x => new AulaConEquiposDTO
				{
					Id = x.IdAula,
					Nombre = x.Nombre,
					Equipos = x.Equipo.Where(x => x.Estado == 1).Select(e=> new EquipoDatosDto
					{
						Id = e.IdEquipo,
						Numero = e.NumeroIdentificacion,
						TipoId = e.TipoEquipoIdTipoEquipo,
						AulaId = e.AulaIdAula,
						Tipo = e.TipoEquipoIdTipoEquipoNavigation.Nombre,
						IconoTipo = ImageToBase64Helper.ConvertBase64($"wwwroot/images/{e.TipoEquipoIdTipoEquipo}.jpg")
                    }).ToList()
				}).FirstOrDefaultAsync();
			return aula;
		}
	}
}
