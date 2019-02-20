using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected void SafeCall(Action action)
        {
            action();
        }

        protected TResult SafeCall<TResult>(Func<TResult> action)
        {
            return action();
        }

    }

    public abstract class BaseRepository<TEntity> : BaseRepository
        where TEntity : BaseModel
    {
        private TailoryfyDbContext _context;

        protected BaseRepository(TailoryfyDbContext context)
        {
            _context = context;
        }

        protected TailoryfyDbContext Context => this._context;

        private DbSet<TEntity> Entity => this.Context.Set<TEntity>();


        public virtual TEntity GetById(int id)
        {
            return SafeCall(() => this.Entity.FirstOrDefault(x => x.Id == id));
        }

        public virtual IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return SafeCall(() => this.Entity.Where(predicate));
        }

        public TEntity Save(TEntity entity)
        {
            return SafeCall(() =>
            {
                if (entity.Id != 0)
                {
                    this.Context.Update(entity);
                }
                else
                {
                    this.Context.Add(entity);
                }
                this.Context.SaveChanges();
                return entity;
            });
        }

        public virtual IEnumerable<TEntity> Get()
        {
            return SafeCall(() => this.Entity);
        }

        protected IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return SafeCall(() => this.Entity.Where(predicate));
        }

        public void Delete(TEntity item)
        {
            SafeCall(() =>
            {
                this.Context.Remove(item);
                this.Context.SaveChanges();
            });
        }

    }
}
