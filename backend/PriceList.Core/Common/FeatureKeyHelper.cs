using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Core.Common
{
    public static class FeatureKeyHelper
    {
        public static string Build(IEnumerable<int> ids) =>
            string.Join("|", ids.Distinct().OrderBy(i => i));
    }
}
