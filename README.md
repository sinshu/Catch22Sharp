# Catch22Sharp

Catch22Sharp is a C# port of the [catch22](https://github.com/chlubba/catch22) feature extraction toolkit. It mirrors the original C implementation so that .NET applications can compute the 22 canonical time-series features that make up the catch22 collection. The repository contains both the original C sources and the managed port, together with a comprehensive regression test suite.

## Repository layout

- `C/` – Reference implementation of the features written in C.
- `Catch22Sharp/` – C# translation of the feature extractors, exposed through the `Catch22` partial class and related helpers.
- `Catch22SharpTest/` – MSTest projects that compare the managed results with the C baseline using fixtures in `testData/`.
- `testData/` – Input samples used by the regression tests.
- `FEATURES.md` – High-level descriptions of the 22 catch22 features.

## Building and running tests

The solution targets the .NET SDK. To restore dependencies, compile, and run the regression tests use:

```bash
dotnet restore
dotnet build
dotnet test
```

The tests load the sample time series from `testData/` and ensure the port matches the numeric output of the original C code.

## Using the library

[The NuGet package](https://www.nuget.org/packages/Catch22Sharp) is available.

```ps1
Install-Package Catch22Sharp
```

All the classes are in the `Catch22Sharp` namespace.

```cs
using Catch22Sharp;
```

__Code example:__
```csharp
var data = ReadData(...);
var catch22 = data.Catch22();
foreach (var (name, value) in catch22.GetNameValuePairs())
{
    Console.WriteLine($"{name}, {value}");
}
```
__Output:__
```
DN_HistogramMode_5, -0.6147991148452683
DN_HistogramMode_10, -0.7822544655522088
DN_OutlierInclude_p_001_mdrmd, 0.40740740740740744
DN_OutlierInclude_n_001_mdrmd, -0.23703703703703705
first1e_acf_tau, 32.50260547693646
firstMin_acf, 77
SP_Summaries_welch_rect_area_5_1, 0.9931387788742457
SP_Summaries_welch_rect_centroid, 0.03681553890925782
FC_LocalSimple_mean3_stderr, 0.08029384289850561
FC_LocalSimple_mean1_tauresrat, 0.8478260869565217
MD_hrv_classic_pnn40, 0.31970260223048325
SB_BinaryStats_mean_longstretch1, 88
SB_BinaryStats_diff_longstretch0, 83
SB_MotifThree_quantile_hh, 1.2105878172438547
CO_HistogramAMI_even_2_5, 1.0063890779937608
CO_trev_1_num, 1.782472611547055E-05
IN_AutoMutualInfoStats_40_gaussian_fmmi, 40
SB_TransitionMatrix_3ac_sumdiagcov, 0.08000000000000003
PD_PeriodicityWang_th0_01, 0
CO_Embed2_Dist_tau_d_expfit_meandiff, 7.135078608788558
SC_FluctAnal_2_rsrangefit_50_1_logi_prop_r1, 0.29545454545454547
SC_FluctAnal_2_dfa_50_1_2_logi_prop_r1, 0.75
```
