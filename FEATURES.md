| #  | Feature name                                 | Short name            | Category                         | Description                                                    |
|----|----------------------------------------------|-----------------------|----------------------------------|----------------------------------------------------------------|
| 1  | `DN_HistogramMode_5`                         | `mode_5`              | Distribution shape               | 5-bin histogram mode                                           |
| 2  | `DN_HistogramMode_10`                        | `mode_10`             | Distribution shape               | 10-bin histogram mode                                          |
| 3  | `DN_OutlierInclude_p_001_mdrmd`              | `outlier_timing_pos`  | Extreme event timing             | Positive outlier timing                                        |
| 4  | `DN_OutlierInclude_n_001_mdrmd`              | `outlier_timing_neg`  | Extreme event timing             | Negative outlier timing                                        |
| 5  | `first1e_acf_tau`                            | `acf_timescale`       | Linear autocorrelation           | First 1/e crossing of the ACF                                  |
| 6  | `firstMin_acf`                               | `acf_first_min`       | Linear autocorrelation           | First minimum of the ACF                                       |
| 7  | `SP_Summaries_welch_rect_area_5_1`           | `low_freq_power`      | Linear autocorrelation           | Power in lowest 20% frequencies                                |
| 8  | `SP_Summaries_welch_rect_centroid`           | `centroid_freq`       | Linear autocorrelation           | Centroid frequency                                             |
| 9  | `FC_LocalSimple_mean3_stderr`                | `forecast_error`      | Simple forecasting               | Error of 3-point rolling mean forecast                         |
| 10 | `FC_LocalSimple_mean1_tauresrat`             | `whiten_timescale`    | Incremental differences          | Change in autocorrelation timescale after differencing         |
| 11 | `MD_hrv_classic_pnn40`                       | `high_fluctuation`    | Incremental differences          | Proportion of high incremental changes in the series           |
| 12 | `SB_BinaryStats_mean_longstretch1`           | `stretch_high`        | Symbolic                         | Longest stretch of above-mean values                           |
| 13 | `SB_BinaryStats_diff_longstretch0`           | `stretch_decreasing`  | Symbolic                         | Longest stretch of decreasing values                           |
| 14 | `SB_MotifThree_quantile_hh`                  | `entropy_pairs`       | Symbolic                         | Entropy of successive pairs in symbolized series               |
| 15 | `CO_HistogramAMI_even_2_5`                   | `ami2`                | Nonlinear autocorrelation        | Histogram-based automutual information (lag 2, 5 bins)         |
| 16 | `CO_trev_1_num`                              | `trev`                | Nonlinear autocorrelation        | Time reversibility                                             |
| 17 | `IN_AutoMutualInfoStats_40_gaussian_fmmi`    | `ami_timescale`       | Linear autocorrelation structure | First minimum of the AMI function                              |
| 18 | `SB_TransitionMatrix_3ac_sumdiagcov`         | `transition_variance` | Symbolic                         | Transition matrix column variance                              |
| 19 | `PD_PeriodicityWang_th001`                   | `periodicity`         | Linear autocorrelation structure | Wang’s periodicity metric                                      |
| 20 | `CO_Embed2_Dist_tau_d_expfit_meandiff`       | `embedding_dist`      | Other                            | Goodness of exponential fit to embedding distance distribution |
| 21 | `SC_FluctAnal_2_rsrangeﬁt_50_1_logi_prop_r1` | `rs_range`            | Self-affine scaling              | Rescaled range fluctuation analysis (low-scale scaling)        |
| 22 | `SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1`     | `dfa`                 | Self-affine scaling              | Detrended fluctuation analysis (low-scale scaling)             |
| 23 | `DN_Mean`                                    | `mean`                | (Catch24 only)                   | Mean                                                           |
| 24 | `DN_Spread_Std`                              | `std`                 | (Catch24 only)                   | Standard deviation                                             |
