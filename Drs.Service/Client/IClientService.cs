using System.Collections.Generic;
using Drs.Model.Order;
using Drs.Model.Shared;

namespace Drs.Service.Client
{
    public interface IClientService
    {
        IEnumerable<ListItemModel> SearchByPhone(string phone);
        IEnumerable<ListItemModel> SearchByCompany(string company);
        IEnumerable<ClientInfoModel> SearchClientsByPhone(string phone);
        IEnumerable<ListItemModel> SearchClientsByClientName(string clientName);
    }
}
