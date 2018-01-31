using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using MrRondon.Domain.Interfaces.Repositories;
using MrRondon.Infra.Data.Context;

namespace MrRondon.Infra.Data.Repositories
{
    public class RepositorioBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected MainContext Context;
        protected DbSet<TEntity> DbSet;

        public RepositorioBase(MainContext mainContext = null)
        {
            Context = mainContext ?? new MainContext();
            DbSet = Context.Set<TEntity>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public bool IsTrue(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Any(expression);
        }

        public virtual IEnumerable<TEntity> GetItemsByExpression(Expression<Func<TEntity, bool>> expression, params string[] objectsToInclude)
        {
            if (objectsToInclude == null) return DbSet.Where(expression);

            DbQuery<TEntity> dbQuery = null;
            foreach (var obj in objectsToInclude)
            {
                if (dbQuery != null) dbQuery.Include(obj);
                else dbQuery = DbSet.Include(obj);
            }

            return dbQuery?.Where(expression) ?? DbSet.Where(expression);
        }

        public TEntity GetItemByExpression(Expression<Func<TEntity, bool>> expression, params string[] objectsToInclude)
        {
            if (objectsToInclude == null) return DbSet.FirstOrDefault(expression);

            DbQuery<TEntity> dbQuery = null;
            foreach (var obj in objectsToInclude)
            {
                if (dbQuery != null) dbQuery.Include(obj);
                else dbQuery = DbSet.Include(obj);
            }
            return dbQuery == null ? DbSet.FirstOrDefault(expression) : dbQuery.FirstOrDefault(expression);
        }

        public virtual IEnumerable<TEntity> GetItemsByExpression(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy, int start, int length, out int recordsTotal, params string[] objectsToInclude)
        {
            recordsTotal = DbSet.Count(expression);
            if (objectsToInclude == null) return DbSet.Where(expression).OrderBy(orderBy).Skip(start).Take(length);

            DbQuery<TEntity> dbQuery = null;
            foreach (var obj in objectsToInclude)
            {
                if (dbQuery != null) dbQuery.Include(obj);
                else dbQuery = DbSet.Include(obj);
            }

            return dbQuery?.Where(expression).OrderBy(orderBy).Skip(start).Take(length) ?? DbSet.Where(expression).OrderBy(orderBy).Skip(start).Take(length);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Context.Dispose();
        }
    }
}