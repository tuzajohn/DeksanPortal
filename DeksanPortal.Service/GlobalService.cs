using DeksanPortal.Core.DataManager.DataRepository;
using DeksanPortal.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeksanPortal.Service
{
    public class GlobalService<TEntity> : IGlobalService<TEntity> where TEntity : class
    {
        private readonly IDeksanRepository<TEntity> _conext;
        public GlobalService(IDeksanRepository<TEntity> conext)
        {
            _conext = conext;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            return await _conext.Add(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entity)
        {
            await _conext.AddRange(entity);
        }

        public async Task Delete(TEntity entity)
        {
            await _conext.Delete(entity);
        }

        public async Task<TEntity> Get(string id)
        {
            return await _conext.Get(id);
        }

        public IQueryable<TEntity> SearchQ(Expression<Func<TEntity, bool>> expression)
        {
            return _conext.SearchQ(expression);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            return await _conext.Update(entity);
        }
    }
}
