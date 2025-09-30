using System;
using System.Collections.Generic;
using System.Linq;

namespace Catch22Sharp
{
    public static class Catch22Ex
    {
        public static Catch22 Catch22(this IEnumerable<double> y)
        {
            return new Catch22(y.ToArray());
        }
    }
}
