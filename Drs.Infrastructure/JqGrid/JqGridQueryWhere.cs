using System.Collections.Generic;

namespace Drs.Infrastructure.JqGrid
{
    public class JqGridQueryWhere
    {
        public JqGridQueryWhere()
        {
            LstParams = new List<object>();
        }

        public string Query { get; set; }
        public List<object> LstParams { get; set; }
        public object[] ArrParams
        {
            get { return LstParams.ToArray(); }
        }

    }
}

