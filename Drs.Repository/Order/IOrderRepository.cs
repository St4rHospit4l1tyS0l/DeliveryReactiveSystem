using System;
using System.Collections.Generic;
using Drs.Model.Address;
using Drs.Model.Client;
using Drs.Model.Order;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public interface IOrderRepository : IBaseOneRepository, IDisposable
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
        int GetPhoneIdByPhone(string phone);
        OrderInfoModel GetPosOrderByOrderToStoreId(long orderToStoreId);
        PosCheck GetPosCheckById(int posOrderId);
        IEnumerable<LastOrderInfoModel> GetLastNthOrdersIdByClientPhoneId(int clientPhoneId);
    }
}
