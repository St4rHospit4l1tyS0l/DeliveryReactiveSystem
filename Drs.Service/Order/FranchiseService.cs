using System.Collections.Generic;
using Drs.Model.Menu;
using Drs.Repository.Order;

namespace Drs.Service.Order
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
    }
}
