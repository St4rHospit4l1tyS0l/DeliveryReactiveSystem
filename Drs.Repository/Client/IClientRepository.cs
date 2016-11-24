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
        long GetPhoneIdByPhone(string phone);
        IEnumerable<ClientInfoModel> SearchClientsByPhoneId(long phoneId);
        ClientInfoModel GetClientById(long clientId);
        string GetPhoneById(long phoneId);
        IEnumerable<ListItemModel> SearchClientsByClientName(string clientName, int maxResultsOnQuery);
        int CountRecurrenceByTime(long dtStart, long dtEnd, long clientId);
        decimal TotalRecurrenceByTotal(long dtStart, long dtEndShort, long clientId);
    }
}
