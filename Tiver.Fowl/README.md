# Tiver.Fowl

A framework for writing Automated Integration tests (including tests via Selenium).

## What it believes

Let a .NET developer write a browser test that reads like the manual test case — focuses on actions to be performed, never goes stale, runs in parallel, and produces a shareable HTML report, with no configuration beyond a JSON file.

## Quick Start

### 1. Configure

Create `Tiver_config.json` in your test project — browser and waiting:

```json
{
  "BrowserConfiguration": {
    "BrowserType": "chrome",
    "Headless": false,
    "Resolution": { "Width": 1920, "Height": 1080 }
  }
}
```

Create `config.json` — application settings, and the named URLs your tests navigate to:

```json
{
  "Urls": {
    "home": "https://your-app.com",
    "cart": "https://your-app.com/cart"
  }
}
```

Both files must be copied to the output directory.

### 2. Create Element Classes

A capability is an interface — `IClickable`, `ITypeable`, `IVisible`, `IHasAttributes` — so an
element declares only what it actually supports:

```csharp
using System.Runtime.CompilerServices;
using Tiver.Fowl.ViewBase;
using Tiver.Fowl.ViewBase.Behaviors;

public class Button : Element, IClickable
{
    public Button(string locator, [CallerMemberName] string name = null) : base(locator, name) { }
}

public class Textbox : Element, ITypeable
{
    public Textbox(string locator, [CallerMemberName] string name = null) : base(locator, name) { }
}
```

### 3. Write a Test

```csharp
[WebDriverTest]
public class MyTests : BaseTestForNUnit
{
    private static readonly Button PhonesMenuItem = new("//a[text()='Phones']");

    [Test]
    public void MyFirstTest()
    {
        ActiveConfiguration.NavigateTo("home");   // tests control their own navigation
        this.LogStep("Open the Phones category");
        PhonesMenuItem.Click();
    }
}
```

Logger configuration, teardown, screenshot-on-failure and HTML report generation need no wiring —
`BaseTestForNUnit` and `BaseTestForMSTest` handle them. The matching base class is enabled
automatically from the test framework you referenced.

## Documentation

For more information, visit: https://github.com/MrHant/tiver-fowl

## License

MIT License - see https://github.com/MrHant/tiver-fowl for details
