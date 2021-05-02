using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DatabaseTransaction
{
    public class DatabaseTransaction : IDatabaseTransaction, IDisposable
    {
        private ServerDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _hasTransactionBegun = false;

        public DatabaseTransaction(ServerDbContext context)
        {
            _context = context;
        }
        public void BeginTransaction()
        {
            _hasTransactionBegun = true;
            _transaction = _context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _transaction.Commit();
        }
        public void RollbackTransaction()
        {
            _transaction.Rollback();
        }
        public void Dispose()
        {
            if(_hasTransactionBegun)
            {
                _transaction.Dispose();
            }
        }
    }
}
