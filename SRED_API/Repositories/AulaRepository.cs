using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class AulaRepository(WebsitosSredContext context): Repository<Aula>(context)
	{
		private readonly WebsitosSredContext Context = context;
		public List<Aula> GetAulas()
		{
			return Context.Aula.ToList();
		}
		public Aula? GetAula(int id)
		{
			return Context.Aula.Include(x=>x.Equipo).FirstOrDefault(x=>x.IdAula == id);
		}
	}
}
