# Overview

This project involves porting the C code called *“catch22”*, which extracts 22 features from time-series data, into C#.

# Original C Code

The source C code is located in the **C** directory.

# Ported C# Code

Place the ported code in the **Catch22Sharp** directory.

# Tests

Unit tests should be placed in the **Catch22SharpTest** directory.
These tests reference the data in the **testData** directory and are meant to verify that the port behaves identically to the original C implementation.
Whenever the code is modified, make sure to run these tests.

# Porting Guidelines

* Port the original C code as faithfully as possible. You may ignore C# conventions if necessary to preserve the original logic. Also, reproduce the original code comments.
* When the C code passes arrays using a pointer and an `int size` pair, replace it with a single `Span` and use `Span.Length` instead of `size`.
* Additionally, use `ReadOnlySpan` where possible.
* For each `.c` file that implements feature calculations, create a `.cs` file with the same name, and port the functions as `static` methods of the `Catch22` partial class.
* For tests, create a `.cs` file per function (named after the function), and base the code on the existing test templates.
