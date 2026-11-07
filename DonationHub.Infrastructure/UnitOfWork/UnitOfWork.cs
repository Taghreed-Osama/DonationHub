using DonationHub.Application.Interfaces.UnitOfWork;
using DonationHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);

        public void Dispose()
            => _context.Dispose();
    }
}
