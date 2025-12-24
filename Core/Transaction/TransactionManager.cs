using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Transaction
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ApplicationDbContext _context; 
        public IDbContextTransaction Transaction { get; private set; }

        public TransactionManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            Transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            await Transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await Transaction.RollbackAsync(cancellationToken);
        }
    }
}
