using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DatabaseTransaction
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private ServerDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _hasTransactionBegun = false;
        private bool _isDone = false;

        public DatabaseTransaction(ServerDbContext context)
        {
            _context = context;
        }
        public IDatabaseTransaction BeginTransaction()
        {
            _hasTransactionBegun = true;
            _transaction = _context.Database.BeginTransaction();
            return this;
        }
        public void CommitTransaction()
        {
            _isDone = true;
            _transaction.Commit();
        }
        public void RollbackTransaction()
        {
            _isDone = true;
            _transaction.Rollback();
        }
        public void Dispose()
        {
            if(_hasTransactionBegun)
            {
                if(!_isDone)
                {
                    RollbackTransaction();
                }
                _transaction.Dispose();
            }
        }
    }
}
