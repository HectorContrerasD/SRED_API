using Microsoft.EntityFrameworkCore;

namespace SRED_API.Repositories
{
    public interface IRepositorycs<T> where T : class
    {
        Task<T?> Get(int id);
        DbSet<T> GetAll();
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
