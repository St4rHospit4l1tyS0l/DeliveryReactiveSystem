using System;
using Drs.Model.Address;
using Drs.Model.Client;
using Drs.Model.Order;

namespace Drs.Repository.Client
{
    public interface IOrderRepository : IDisposable
    {
        PhoneModel GetPhoneByPhone(string phone);
        PhoneModel SavePhone(PhoneModel model);
        int? FindCompanyByName(string companyName);
        int SaveCompany(string companyName);
        int SaveClient(ClientInfoModel model, bool b);
        bool RelPhoneClientExists(ClientPhoneModel model);
        void RemoveRelPhoneClient(ClientPhoneModel model);
        bool RelPhoneAddressExists(AddressPhoneModel model);
        void RemoveRelPhoneAddress(AddressPhoneModel model);
        int? SavePosCheck(PosCheck model);
    }
}
