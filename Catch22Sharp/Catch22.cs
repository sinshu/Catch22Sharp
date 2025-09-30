using System;
using System.Collections;
using System.Collections.Generic;

namespace Catch22Sharp
{
    /// <summary>
    /// Provides convenient access to the 22 canonical time-series features defined by the catch22 toolkit.
    /// </summary>
    public partial class Catch22 : IReadOnlyList<double>
    {
        private static readonly Dictionary<string, int> nameToIndex;

        static Catch22()
        {
            nameToIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "DN_HistogramMode_5", 0 },
                { "mode_5", 0 },

                { "DN_HistogramMode_10", 1 },
                { "mode_10", 1 },

                { "DN_OutlierInclude_p_001_mdrmd", 2 },
                { "outlier_timing_pos", 2 },

                { "DN_OutlierInclude_n_001_mdrmd", 3 },
                { "outlier_timing_neg", 3 },

                { "first1e_acf_tau", 4 },
                { "acf_timescale", 4 },
                { "CO_f1ecac", 4 },

                { "firstMin_acf", 5 },
                { "acf_first_min", 5 },
                { "CO_FirstMin_ac", 5 },

                { "SP_Summaries_welch_rect_area_5_1", 6 },
                { "low_freq_power", 6 },

                { "SP_Summaries_welch_rect_centroid", 7 },
                { "centroid_freq", 7 },

                { "FC_LocalSimple_mean3_stderr", 8 },
                { "forecast_error", 8 },

                { "FC_LocalSimple_mean1_tauresrat", 9 },
                { "whiten_timescale", 9 },

                { "MD_hrv_classic_pnn40", 10 },
                { "high_fluctuation", 10 },

                { "SB_BinaryStats_mean_longstretch1", 11 },
                { "stretch_high", 11 },

                { "SB_BinaryStats_diff_longstretch0", 12 },
                { "stretch_decreasing", 12 },

                { "SB_MotifThree_quantile_hh", 13 },
                { "entropy_pairs", 13 },

                { "CO_HistogramAMI_even_2_5", 14 },
                { "ami2", 14 },

                { "CO_trev_1_num", 15 },
                { "trev", 15 },

                { "IN_AutoMutualInfoStats_40_gaussian_fmmi", 16 },
                { "ami_timescale", 16 },

                { "SB_TransitionMatrix_3ac_sumdiagcov", 17 },
                { "transition_variance", 17 },

                { "PD_PeriodicityWang_th0_01", 18 },
                { "periodicity", 18 },

                { "CO_Embed2_Dist_tau_d_expfit_meandiff", 19 },
                { "embedding_dist", 19 },

                { "SC_FluctAnal_2_rsrangefit_50_1_logi_prop_r1", 20 },
                { "rs_range", 20 },

                { "SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1", 21 },
                { "dfa", 21 },
            };
        }

        private readonly double[] values;

        /// <summary>
        /// Computes the catch22 feature set for the supplied time-series sample.
        /// </summary>
        /// <param name="y">Input time-series data expressed as a span of doubles.</param>
        public Catch22(ReadOnlySpan<double> y)
        {
            var normalized = y.ToArray();
            Stats.zscore_norm2(y, normalized);
            y = normalized;

            values = new double[22];
            values[0] = DN_HistogramMode_5(y);
            values[1] = DN_HistogramMode_10(y);
            values[2] = DN_OutlierInclude_p_001_mdrmd(y);
            values[3] = DN_OutlierInclude_n_001_mdrmd(y);
            values[4] = CO_f1ecac(y);
            values[5] = CO_FirstMin_ac(y);
            values[6] = SP_Summaries_welch_rect_area_5_1(y);
            values[7] = SP_Summaries_welch_rect_centroid(y);
            values[8] = FC_LocalSimple_mean3_stderr(y);
            values[9] = FC_LocalSimple_mean1_tauresrat(y);
            values[10] = MD_hrv_classic_pnn40(y);
            values[11] = SB_BinaryStats_mean_longstretch1(y);
            values[12] = SB_BinaryStats_diff_longstretch0(y);
            values[13] = SB_MotifThree_quantile_hh(y);
            values[14] = CO_HistogramAMI_even_2_5(y);
            values[15] = CO_trev_1_num(y);
            values[16] = IN_AutoMutualInfoStats_40_gaussian_fmmi(y);
            values[17] = SB_TransitionMatrix_3ac_sumdiagcov(y);
            values[18] = PD_PeriodicityWang_th0_01(y);
            values[19] = CO_Embed2_Dist_tau_d_expfit_meandiff(y);
            values[20] = SC_FluctAnal_2_rsrangefit_50_1_logi_prop_r1(y);
            values[21] = SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1(y);
        }

        /// <summary>
        /// Gets the feature value corresponding to the specified feature index.
        /// </summary>
        /// <param name="index">The zero-based index of the feature.</param>
        /// <returns>
        /// The computed feature value.
        /// </returns>
        public double this[int index]
        {
            get
            {
                if ((uint)index >= (uint)values.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return values[index];
            }
        }

        /// <summary>
        /// Gets the feature value corresponding to the specified feature name.
        /// </summary>
        /// <param name="featureName">The canonical or short feature identifier.</param>
        /// <returns>
        /// The computed feature value.
        /// </returns>
        public double this[string featureName]
        {
            get
            {
                if (featureName is null)
                {
                    throw new ArgumentNullException(nameof(featureName));
                }

                if (!nameToIndex.TryGetValue(featureName, out var featureIndex))
                {
                    throw new ArgumentException($"Unknown feature name: {featureName}", nameof(featureName));
                }

                return values[featureIndex];
            }
        }

        /// <summary>
        /// doc comment here
        /// </summary>
        /// <returns>
        /// doc comment here
        /// </returns>
        public (string Name, double Value)[] GetNameValuePairs()
        {
            throw new NotImplementedException();
        }

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
        /// [Distribution shape] 5-bin histogram mode
        /// </summary>
        public double Mode5 => values[0];

        /// <summary>
        /// [Distribution shape] 10-bin histogram mode
        /// </summary>
        public double Mode10 => values[1];

        /// <summary>
        /// [Extreme event timing] Positive outlier timing
        /// </summary>
        public double OutlierTimingPos => values[2];

        /// <summary>
        /// [Extreme event timing] Negative outlier timing
        /// </summary>
        public double OutlierTimingNeg => values[3];

        /// <summary>
        /// [Linear autocorrelation] First 1/e crossing of the ACF
        /// </summary>
        public double AcfTimescale => values[4];

        /// <summary>
        /// [Linear autocorrelation] First minimum of the ACF
        /// </summary>
        public double AcfFirstMin => values[5];

        /// <summary>
        /// [Linear autocorrelation] Power in lowest 20% frequencies
        /// </summary>
        public double LowFreqPower => values[6];

        /// <summary>
        /// [Linear autocorrelation] Centroid frequency
        /// </summary>
        public double CentroidFreq => values[7];

        /// <summary>
        /// [Simple forecasting] Error of 3-point rolling mean forecast
        /// </summary>
        public double ForecastError => values[8];

        /// <summary>
        /// [Incremental differences] Change in autocorrelation timescale after differencing
        /// </summary>
        public double WhitenTimescale => values[9];

        /// <summary>
        /// [Incremental differences] Proportion of high incremental changes in the series
        /// </summary>
        public double HighFluctuation => values[10];

        /// <summary>
        /// [Symbolic] Longest stretch of above-mean values
        /// </summary>
        public double StretchHigh => values[11];

        /// <summary>
        /// [Symbolic] Longest stretch of decreasing values
        /// </summary>
        public double StretchDecreasing => values[12];

        /// <summary>
        /// [Symbolic] Entropy of successive pairs in symbolized series
        /// </summary>
        public double EntropyPairs => values[13];

        /// <summary>
        /// [Nonlinear autocorrelation] Histogram-based automutual information (lag 2, 5 bins)
        /// </summary>
        public double Ami2 => values[14];

        /// <summary>
        /// [Nonlinear autocorrelation] Time reversibility
        /// </summary>
        public double Trev => values[15];

        /// <summary>
        /// [Linear autocorrelation structure] First minimum of the AMI function
        /// </summary>
        public double AmiTimescale => values[16];

        /// <summary>
        /// [Symbolic] Transition matrix column variance
        /// </summary>
        public double TransitionVariance => values[17];

        /// <summary>
        /// [Linear autocorrelation structure] Wang's periodicity metric
        /// </summary>
        public double Periodicity => values[18];

        /// <summary>
        /// [Other] Goodness of exponential fit to embedding distance distribution
        /// </summary>
        public double EmbeddingDist => values[19];

        /// <summary>
        /// [Self-affine scaling] Rescaled range fluctuation analysis (low-scale scaling)
        /// </summary>
        public double RsRange => values[20];

        /// <summary>
        /// [Self-affine scaling] Detrended fluctuation analysis (low-scale scaling)
        /// </summary>
        public double Dfa => values[21];
    }
}
