using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class AulaRepository(WebsitosSredContext context): Repository<Aula>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public async Task<List<Aula>> GetAulas()
		{
			return await Context.Aula.ToListAsync();
		}
		public async Task<Aula?> GetAula(int id)
		{
			return await Context.Aula.Include(x=>x.Equipo).FirstOrDefaultAsync(x=>x.IdAula == id);
		}
	}
}
