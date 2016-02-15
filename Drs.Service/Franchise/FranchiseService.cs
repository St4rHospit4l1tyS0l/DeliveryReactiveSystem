using System.Collections.Generic;
using Drs.Infrastructure.Crypto;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Order;

namespace Drs.Service.Franchise
{
    public class FranchiseService : IFranchiseService
    {
        private readonly IFranchiseRepository _repository;
        public FranchiseService(IFranchiseRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<ButtonItemModel> GetFranchiseButtons()
        {
            using (_repository)
            {
                return _repository.GetFranchiseButtons();
            }
        }

        public IEnumerable<SyncFranchiseModel> GetListSyncFiles(string sHost)
        {
            var eInfo = Cypher.Encrypt(sHost);

            using (_repository)
            {
                var lstFranchiseFiles = _repository.GetListSyncFiles(eInfo);
                return lstFranchiseFiles;
            }
        }

        public IEnumerable<OptionModel> LstFranchise()
        {
            return _repository.LstFranchise();
        }
    }
}
