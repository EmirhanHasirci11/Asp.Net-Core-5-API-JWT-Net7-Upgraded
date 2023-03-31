using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> Where(Expression<Func<T,bool>>expression);
        Task AddAsync(T entity);
        void Remove(T entity);
        T Update(T entity);

    }
}
