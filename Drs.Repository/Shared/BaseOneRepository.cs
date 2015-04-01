using System;
using Drs.Repository.Entities;

namespace Drs.Repository.Shared
{
    public abstract class BaseOneRepository : IDisposable
    {
        protected readonly CallCenterEntities DbEntities;

        protected BaseOneRepository()
        {
            DbEntities = new CallCenterEntities();
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
