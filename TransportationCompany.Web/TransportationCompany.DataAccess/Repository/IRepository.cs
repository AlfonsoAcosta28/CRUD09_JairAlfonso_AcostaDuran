using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportationCompany.DataAccess.Repository
{
    public interface IRepository<TId, TEntity> where TEntity : class, new()
    {
        TransportationCompanyContext Context { get; }

        IQueryable<TEntity> GetAll();
        Task<TEntity> GetAsync(TId id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
    }
}
