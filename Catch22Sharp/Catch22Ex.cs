using System;
using System.Collections.Generic;
using System.Linq;

namespace Catch22Sharp
{
    /// <summary>
    /// Provides extension methods that create <see cref="Catch22"/> analyzers for
    /// numeric sequences.
    /// </summary>
    public static class Catch22Ex
    {
        /// <summary>
        /// Creates a <see cref="Catch22"/> instance from the specified sequence so its
        /// statistical features can be computed.
        /// </summary>
        /// <param name="y">The time-series data to analyze.</param>
        /// <returns>The <see cref="Catch22"/> analyzer for the provided data.</returns>
        public static Catch22 Catch22(this IEnumerable<double> y)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            var array = y.ToArray();
            if (array.Length == 0)
            {
                throw new ArgumentException("The time-series must contain at least one element.", nameof(y));
            }

            return new Catch22(array);
        }
    }
}
