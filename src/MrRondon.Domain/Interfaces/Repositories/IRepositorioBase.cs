using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MrRondon.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        bool IsTrue(Expression<Func<TEntity, bool>> expression);
        TEntity GetItemByExpression(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] objectsToInclude);
        IEnumerable<TEntity> GetItemsByExpression(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] objectsToInclude);
        IEnumerable<TEntity> GetItemsByExpression(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy, int start, int length, out int recordsTotal, params Expression<Func<TEntity, object>>[] objectsToInclude);
    }
}