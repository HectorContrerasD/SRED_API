using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRED_API.Models.DTOs;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class AulaRepository(WebsitosSredContext context): Repository<Aula>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task<List<Aula>> GetAulas()
		{
			return await Context.Aula.Include(x=>x.Equipo).ToListAsync();
		}
		public async Task<AulaDTO?> GetAula(int id)
		{
			return await Context.Aula.Select(x=> new AulaDTO
			{
				Id = id,
				Nombre = x.Nombre,
			}).FirstOrDefaultAsync(x=>x.Id == id);
		}
	}
}
