using System;

namespace Drs.Repository.Shared
{
    public interface IResourceRepository : IBaseOneRepository, IDisposable
    {
        void Save(string originalFileName, Guid uidFileName, string pathFileName, string userId, int fileType, string checkSum);
    }
}
