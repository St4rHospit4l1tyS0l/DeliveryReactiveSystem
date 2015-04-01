using System;
using Drs.Repository.Entities;
using Drs.Repository.Log;

namespace Drs.Repository.Shared
{
    public abstract class BaseRepository : IDisposable
    {
        public CallCenterEntities DbConn;

        protected BaseRepository()
        {
            DbConn = new CallCenterEntities();
        }

        protected BaseRepository(CallCenterEntities dbConn)
        {
            DbConn = dbConn;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposing)
                    return;

                if (DbConn != null)
                    DbConn.Dispose();
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex);
            }
            finally
            {
                DbConn = null;
            }
        }

    }
}
