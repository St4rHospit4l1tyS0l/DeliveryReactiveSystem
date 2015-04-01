using System;
using System.Collections.Generic;
using Drs.Model.Order;
using Drs.Model.Shared;

namespace Drs.Repository.Client
{
    public interface IClientRepository : IDisposable
    {
        IEnumerable<ListItemModel> SearchByPhone(string phone, int maxResults);
        IEnumerable<ListItemModel> SearchByCompany(string company, int maxResultsOnQuery);
        int GetPhoneIdByPhone(string phone);
        IEnumerable<ClientInfoModel> SearchClientsByPhoneId(int phoneId);
        ClientInfoModel GetClientById(int clientId);
        string GetPhoneById(int phoneId);
        IEnumerable<ListItemModel> SearchClientsByClientName(string clientName, int maxResultsOnQuery);
    }
}
