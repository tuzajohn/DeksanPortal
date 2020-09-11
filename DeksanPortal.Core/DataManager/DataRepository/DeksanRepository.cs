using DeksanPortal.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeksanPortal.Core.DataManager.DataRepository
{
    public class DeksanRepository<TEntity> : IDeksanRepository<TEntity> where TEntity : class
    {
        private readonly DbContextFactory _factory;

        public DeksanRepository(DbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            using var context = _factory.GetContext();
            await context.AddAsync(entity);
            var c = await context.SaveChangesAsync();
            if (c > 0)
            {
                return entity;
            }
            return default;
        }

        public async Task AddRange(IEnumerable<TEntity> entity)
        {
            using var context = _factory.GetContext();
            await context.AddRangeAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            using var context = _factory.GetContext();
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> Get(string id)
        {
            using var context = _factory.GetContext();
            var TEntity = await context.Set<TEntity>().FindAsync(id);
            return TEntity;
        }

        public IQueryable<TEntity> SearchQ(Expression<Func<TEntity, bool>> expression)
        {
            var TEntities = _factory.GetContext().Set<TEntity>().AsQueryable().Where(expression);
            return TEntities;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            using var context = _factory.GetContext();
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
