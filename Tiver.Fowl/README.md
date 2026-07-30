# Tiver.Fowl

A framework for writing Automated Integration tests (including tests via Selenium).

## What it believes

Let a .NET developer write a browser test that reads like the manual test case — focuses on actions to be performed, never goes stale, runs in parallel, and produces a shareable HTML report, with no configuration beyond a JSON file.


## Quick Start

### 1. Create Test Class

```csharp
[WebDriverTest]
public class MyTests : BaseTestForNUnit
{
    [Test]
    public void MyFirstTest()
    {
        this.LogStep("Navigate to page");
        // Your test code here
    }
}
```

### 2. Configure Browser

Create `Tiver_config.json` in your test project:

```json
{
  "BrowserConfiguration": {
    "BrowserType": "chrome",
    "Headless": false,
    "Resolution": { "Width": 1920, "Height": 1080 }
  }
}
```

Create `config.json`:

```json
{
  "StartUrl": "https://your-app.com"
}
```

### 3. Setup Logger (Optional)

In your test setup fixture:

```csharp
[SetUpFixture]
public class TestSetup
{
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        Logger.Configure();
    }
}
```

### 4. Create Element Classes (Optional)

```csharp
using Tiver.Fowl.ViewBase;
using Tiver.Fowl.ViewBase.Behaviors;

public class Button : Element, IClickable
{
    public Button(string locator) : base(locator) { }
    public Button(string locator, string name) : base(locator, name) { }
}

public class Textbox : Element, ITypeable
{
    public Textbox(string locator) : base(locator) { }
    public Textbox(string locator, string name) : base(locator, name) { }
}
```

## Documentation

For more information, visit: https://github.com/MrHant/tiver-fowl

## License

MIT License - see https://github.com/MrHant/tiver-fowl for details
