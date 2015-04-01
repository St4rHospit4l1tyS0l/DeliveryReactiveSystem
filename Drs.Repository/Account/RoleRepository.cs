using System.Collections.Generic;
using System.Linq;
using Drs.Model.Shared;
using Drs.Repository.Shared;

namespace Drs.Repository.Account
{
    public class RoleRepository : BaseRepository
    {
        public List<OptionModel> FindAll()
        {
            return DbConn.AspNetRoles.Select(e => new OptionModel
            {
                StKey = e.Id,
                Name = e.Description
            }).ToList();
        }
    }
}
