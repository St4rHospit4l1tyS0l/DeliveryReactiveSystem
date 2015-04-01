using System.Collections.Generic;

namespace Infrastructure.JqGrid
{
    public class JqGridQueryWhere
    {
        public string Query { get; set; }
        public List<object> LstParams { get; set; }
        public object[] ArrParams
        {
            get { return LstParams.ToArray(); }
        }

    }
}

