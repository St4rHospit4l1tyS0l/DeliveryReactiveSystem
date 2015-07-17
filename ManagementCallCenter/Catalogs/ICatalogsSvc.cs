using System.ServiceModel;
using Drs.Model.Settings;

namespace ManagementCallCenter.Catalogs
{
    [ServiceContract]
    public interface ICatalogsSvc
    {
        [OperationContract]
        ResponseCatalogs FindAllCatalogs();
    }
}
