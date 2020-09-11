using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeksanPortal.Core.DataManager.DataRepository
{
    public interface IDeksanRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Get(string id);
        Task Delete(TEntity entity);
        IQueryable<TEntity> SearchQ(Expression<Func<TEntity, bool>> expression);
    }
}
