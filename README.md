# Diffa

[![NuGet](https://img.shields.io/nuget/v/Acklann.Diffa.svg)](https://www.nuget.org/packages/Acklann.Diffa/)
[![NuGet](https://img.shields.io/nuget/dt/Acklann.Diffa.svg)](https://www.nuget.org/packages/Acklann.Diffa/)

## What it is?

Diffa is a unit-test assertion library that allows you to verify your test results against approved files. Inspired by [ApprovalTests](https://github.com/approvals/ApprovalTests.Net), Diffa will also automatically launch your favorite diff tool when an assertion fails so you can compare approve the results.

## Basic Use

```c#
[TestClass]
[Use(typeof(DiffReporter))]
[SaveFilesAt("approved-results/")]
public class UnitTests
{
    [TestMethod]
    public void Ensure_results_is_well_formed()
    {
        Diffa.Approve("This is my test results.");
    }
}
```

## Installation

*Available at:*

NUGET: `PM> Install-Package Acklann.Diffa`

## Contributing