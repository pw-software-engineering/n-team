using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Database.DatabaseTransaction
{
    public interface IDatabaseTransaction : IDisposable
    {
        IDatabaseTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
