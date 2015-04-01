using System.Collections.Generic;

namespace Drs.Infrastructure.JqGrid.Model
{
    public class JqGridResultModel
    {

        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public List<JqGridRowsModel> rows { get; set; }

    }
}

