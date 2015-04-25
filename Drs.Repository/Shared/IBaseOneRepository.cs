using Drs.Repository.Entities;

namespace Drs.Repository.Shared
{
    public interface IBaseOneRepository
    {
        CallCenterEntities Db { get; }
    }
}