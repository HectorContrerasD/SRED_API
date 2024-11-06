using Microsoft.EntityFrameworkCore;
using SRED_API.Models.Entities;

namespace SRED_API.Repositories
{
	public class Repository<T>(WebsitosSredContext context) : IRepositorycs<T> where T : class
	{
       
        public WebsitosSredContext Context { get; }


        public async Task<T?> Get(int id)
        {
            return await context.FindAsync<T>(id);  
        }

        public DbSet<T> GetAll()
        {
           return context.Set<T>(); 
        }

        public async Task Insert(T entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();   
        }

        public async Task Update(T entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
