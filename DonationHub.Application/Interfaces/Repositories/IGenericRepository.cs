using DonationHub.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DonationHub.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(
           Expression<Func<TEntity, bool>> predicate,
           CancellationToken ct = default);
        Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken ct = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
