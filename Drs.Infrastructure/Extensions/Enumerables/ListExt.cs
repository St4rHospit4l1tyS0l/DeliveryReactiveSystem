using System.Collections.Generic;
using ReactiveUI;

namespace Drs.Infrastructure.Extensions.Enumerables
{
    public static class ListExt
    {
        public static void ClearAndAddRange<T>(this IReactiveList<T> lstSource, IEnumerable<T> lstAdditional)
        {
            if(lstAdditional == null)
                return;

            lstSource.Clear();
            foreach (var item in lstAdditional)
            {
                lstSource.Add(item);
            }
        }
    }
}
