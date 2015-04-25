using System.Collections.Generic;
using System.Linq;
using Drs.Model.Franchise;
using Drs.Model.Menu;
using Drs.Model.Shared;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public class FranchiseRepository : BaseOneRepository, IFranchiseRepository
    {
        public FranchiseRepository()
        {
            
        }

        public FranchiseRepository(CallCenterEntities db)
            :base(db)
        {
            
        }

        public IEnumerable<ButtonItemModel> GetFranchiseButtons()
        {
            return DbEntities.Franchise.Where(e => e.IsObsolete == false).Select(e => new ButtonItemModel
                    {
                        Color = e.FranchiseButton.Color,
                        Title = e.ShortName,
                        Position = e.FranchiseButton.Position,
                        Image = e.FranchiseButton.Image,
                        Code = e.Code,
                        Description = e.FranchiseButton.Products,
                        DataInfo = new FranchiseDataModel{DataFolder = e.FranchiseData.DataFolder, NewDataFolder = e.FranchiseData.NewDataFolder}
                    }).OrderBy(e => e.Position).ToList();

        }

        public OptionModel GetFranchiseByCode(string franchiseCode)
        {
            return DbEntities.Franchise.Where(e => e.Code == franchiseCode).Select(e => new OptionModel
            {
                StKey = e.FranchiseId.ToString(),
                Name = e.Name
            }).FirstOrDefault();
        }
    }
}
