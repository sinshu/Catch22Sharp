using System;
using System.Collections;
using System.Collections.Generic;

namespace Catch22Sharp
{
    /// <summary>
    /// Add comment here
    /// </summary>
    public partial class Catch22 : IReadOnlyList<double>
    {
        private static Dictionary<string, int> nameToIndex;

        static Catch22()
        {
            nameToIndex = new Dictionary<string, int>
            {
                { "DN_HistogramMode_5", 0 },
                { "mode_5", 0 },
                { "DN_HistogramMode_10", 1 },
                { "mode_10", 1 },
            };
        }

        private double[] values;

        /// <summary>
        /// Add comment here
        /// </summary>
        /// <param name="y">
        /// Add comment here
        /// </param>
        public Catch22(ReadOnlySpan<double> y)
        {
            values = new double[22];
            values[0] = DN_HistogramMode_5(y);
            values[1] = DN_HistogramMode_10(y);
        }

        /// <inheritdoc/>
        public double this[int index] => values[index];

        /// <summary>
        /// Add comment here
        /// </summary>
        /// <param name="featureName">
        /// Add comment here
        /// </param>
        /// <returns>
        /// Add comment here
        /// </returns>
        public double this[string featureName] => values[nameToIndex[featureName]];

        /// <inheritdoc/>
        public IEnumerator<double> GetEnumerator()
        {
            return ((IEnumerable<double>)values).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public int Count => 22;

        /// <summary>
        /// (Distribution shape) 5-bin histogram mode
        /// </summary>
        public double Mode5 => values[0];

        /// <summary>
        /// (Distribution shape) 10-bin histogram mode
        /// </summary>
        public double Mode10 => values[1];
    }
}
