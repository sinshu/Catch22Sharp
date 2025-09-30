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

Add a project reference to `Catch22Sharp/Catch22Sharp.csproj` from your .NET application. The `Catch22` partial class exposes static methods for each feature, accepting `ReadOnlySpan<double>` inputs where appropriate. For example, to evaluate the autocorrelation feature for a sequence:

```csharp
using Catch22Sharp;

ReadOnlySpan<double> series = stackalloc double[] { 0.8, 0.4, -0.2, 0.5, 0.1 };
ReadOnlySpan<int> tau = stackalloc int[] { 1 };
double autocorrLag1 = Catch22.CO_AutoCorr(series, tau)[0];
```

Refer to `FEATURES.md` for background on each feature and the `Catch22Sharp` sources for the exact signatures and implementation details.
