# tiver-fowl  ![.NET](https://img.shields.io/badge/.NET-10-blue) ![C#](https://img.shields.io/badge/C%23-14-blue) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/MrHant/tiver-fowl/master/LICENSE)

[![NuGet](https://img.shields.io/nuget/v/Tiver.Fowl.svg?label=stable)](https://www.nuget.org/packages/Tiver.Fowl) [![NuGet Pre Release](https://img.shields.io/nuget/vpre/Tiver.Fowl.svg?label=pre-release)](https://www.nuget.org/packages/Tiver.Fowl/absoluteLatest)

A framework for writing Automated Integration tests (including tests via Selenium). Provides an XPath
element abstraction with built-in wait/retry, JSON configuration, thread-safe context for parallel
runs, Serilog logging, and an HTML report generated after every run.

## Documentation

| Doc | Covers |
| --- | ------ |
| [docs/CONFIGURATION.md](docs/CONFIGURATION.md) | `Tiver_config.json`, `config.json`, environment layering, `ActiveConfiguration` API, driver management, wait/retry |
| [docs/USAGE.md](docs/USAGE.md) | Elements, behaviors, locators, page objects, browser actions, context and storage |
| [docs/REPORTING.md](docs/REPORTING.md) | Serilog logging, HTML report generation, templates, troubleshooting |

## Installation

Requires .NET SDK 10.0.100 or later, and Chrome or Firefox for browser-driven tests — drivers are
handled by Selenium Manager by default.

Add to your test project, from the NuGet UI or with `dotnet add package <id>`:

- `Tiver.Fowl`
- A test framework — `NUnit` + `NUnit3TestAdapter`, or `MSTest.TestFramework` + `MSTest.TestAdapter`
- `Microsoft.NET.Test.Sdk`
- `Serilog`, `Serilog.Extensions.Logging`, `Serilog.Sinks.Console`, `Serilog.Sinks.File`

Then add `Tiver_config.json` (browser, waiting) and `config.json` (application settings, named URLs)
to the project, both copied to the output directory — see
[docs/CONFIGURATION.md](docs/CONFIGURATION.md).

Tests inherit `BaseTestForNUnit` or `BaseTestForMSTest`; the matching one is enabled automatically
from the test framework you referenced. Mark classes that need a browser with `[WebDriverTest]`:

```csharp
[WebDriverTest]
public class CatalogTests : BaseTestForNUnit
{
    private static readonly Button LaptopsMenuItem = new("//a[text()='Laptops']", "Laptops");

    [Test]
    public void SelectCategory()
    {
        ActiveConfiguration.NavigateTo("home");   // tests control their own navigation
        this.LogStep("Open 'Laptops' catalog section");
        LaptopsMenuItem.Click();
    }
}
```

Logger setup, teardown and report generation need no wiring — the base classes handle them. See
[docs/USAGE.md](docs/USAGE.md) for elements, page objects and context.

