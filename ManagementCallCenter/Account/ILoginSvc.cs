using System.ServiceModel;
using Drs.Model.Account;
using Drs.Model.Shared;

namespace ManagementCallCenter.Account
{
    [ServiceContract]
    public interface ILoginSvc
    {
        [OperationContract]
        ResponseMessage Login(LoginModel login);
    }
}
