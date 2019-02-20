using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> Get();

        TEntity GetById(int id);

        TEntity Save(TEntity entity);

        void Delete(TEntity item);

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate);
    }
}
