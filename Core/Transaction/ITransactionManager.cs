using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Autofac;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Transaction
{
    public interface ITransactionManager : IScopedDependency
    {
        IDbContextTransaction Transaction { get; }
        
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
