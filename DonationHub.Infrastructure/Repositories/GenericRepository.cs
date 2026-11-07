using DonationHub.Application.Interfaces.Repositories;
using DonationHub.Domain.Common;
using DonationHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DonationHub.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => _dbSet.FindAsync(id, ct).AsTask();

        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(predicate, ct);

        public async Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync();

        public void Add(TEntity entity)
            => _dbSet.Add(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);
        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);
    }
}
