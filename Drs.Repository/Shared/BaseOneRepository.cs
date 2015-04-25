using System;
using Drs.Repository.Entities;

namespace Drs.Repository.Shared
{
    public abstract class BaseOneRepository : IDisposable
    {
        protected readonly CallCenterEntities DbEntities;

        public CallCenterEntities Db {
            get
            {
                return DbEntities;
            }
        }

        protected BaseOneRepository()
        {
            DbEntities = new CallCenterEntities();
        }
        protected BaseOneRepository(CallCenterEntities dbEntities)
        {
            DbEntities = dbEntities;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbEntities != null)
                    DbEntities.Dispose();
            }
        }
    }
}
