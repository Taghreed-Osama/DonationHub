using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
