namespace Drs.Infrastructure.JqGrid.Model
{
    public class JqGridFilterModel
    {
        public string sidx { get; set; }
        public string sord { get; set; }
        public int? page { get; set; }
        public int? rows { get; set; }
        public bool _search { get; set; }
        public string searchField { get; set; }
        public string searchOper { get; set; }
        public string searchString { get; set; }
        public string filters { get; set; }
        public long primaryId { get; set; }
        public string codeItem { get; set; }
    }
}

