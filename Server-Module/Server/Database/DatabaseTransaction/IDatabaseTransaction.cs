using System;

namespace Server.Database.DatabaseTransaction
{
    public interface IDatabaseTransaction : IDisposable
    {
        IDatabaseTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
