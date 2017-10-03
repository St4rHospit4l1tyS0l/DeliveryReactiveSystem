using Drs.Infrastructure.Extensions;

namespace Drs.Model.Store
{
    public class EmailClientOrder
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public string GetInfo()
        {
            return string.Format("{0}{1}", FullName, StringExt.AddIfNotNull(Email));
        }
    }
}