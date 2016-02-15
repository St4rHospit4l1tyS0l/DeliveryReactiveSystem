using System;
using System.Collections.Generic;
using Drs.Infrastructure.Resources;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public interface IFranchiseRepository : IBaseOneRepository, IDisposable
    {
        IEnumerable<ButtonItemModel> GetFranchiseButtons();
        bool IsPositionAlreadyUsed(int position, int franchiseId);
        void Add(FranchiseUpModel model);
        void Update(FranchiseUpModel model);
        Franchise FindById(int id);
        void DoObsolete(Franchise model, string userId);
        bool IsCodeAlreadyUsed(string code, int franchiseId);
        void SaveFranchiseDataVersion(FranchiseDataVersion model);
        string GetUrlSyncWsByFranchiseId(int franchiseId);
        void DoObsoleteVersion(int id, string userId, ResponseMessageModel response);
        IEnumerable<SyncFranchiseModel> GetListSyncFiles(string eInfo);
        List<OptionModel> LstFranchise();
    }
}
